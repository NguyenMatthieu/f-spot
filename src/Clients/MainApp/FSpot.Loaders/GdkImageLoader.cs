//
// GdkImageLoader.cs
//
// Author:
//   Daniel Köb <daniel.koeb@peony.at>
//   Stephane Delcroix <stephane@delcroix.org>
//   Ruben Vermeersch <ruben@savanne.be>
//
// Copyright (C) 2014 Daniel Köb
// Copyright (C) 2009-2010 Novell, Inc.
// Copyright (C) 2009 Stephane Delcroix
// Copyright (C) 2009-2010 Ruben Vermeersch
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED AS IS, WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using System.Threading;

using Gdk;

using FSpot.Utils;
using FSpot.Imaging;

using Hyena;

using TagLib.Image;

namespace FSpot.Loaders
{
	public class GdkImageLoader : Gdk.PixbufLoader, IImageLoader
	{
#region public api
		public GdkImageLoader () : base ()
		{
		}

		~GdkImageLoader ()
		{
			if (!is_disposed) {
				Dispose ();
			}
		}

		public void Load (SafeUri uri)
		{
			if (is_disposed)
				return;

			//First, send a thumbnail if we have one
			if ((thumb = XdgThumbnailSpec.LoadThumbnail (uri, ThumbnailSize.Large, null)) != null) {
				pixbuf_orientation = ImageOrientation.TopLeft;
				EventHandler<AreaPreparedEventArgs> prep = AreaPrepared;
				if (prep != null) {
					prep (this, new AreaPreparedEventArgs (true));
				}
				EventHandler<AreaUpdatedEventArgs> upd = AreaUpdated;
				if (upd != null) {
					upd (this, new AreaUpdatedEventArgs (new Rectangle (0, 0, thumb.Width, thumb.Height)));
				}
			}

			using (var image_file = ImageFile.Create (uri)) {
				image_stream = image_file.PixbufStream ();
				pixbuf_orientation = image_file.Orientation;
			}

			loading = true;
			// The ThreadPool.QueueUserWorkItem hack is there cause, as the bytes to read are present in the stream,
			// the Read is CompletedAsynchronously, blocking the mainloop
			image_stream.BeginRead (buffer, 0, count, delegate (IAsyncResult r) {
				ThreadPool.QueueUserWorkItem (delegate {
					HandleReadDone (r);});
			}, null);
		}

		new public event EventHandler<AreaPreparedEventArgs> AreaPrepared;
		new public event EventHandler<AreaUpdatedEventArgs> AreaUpdated;
		public event EventHandler Completed;

		Pixbuf thumb;

		new public Pixbuf Pixbuf {
			get {
				if (thumb != null) {
					return thumb;
				}
				return base.Pixbuf;
			}
		}

		bool loading = false;

		public bool Loading {
			get { return loading; }
		}

		bool notify_prepared = false;
		bool prepared = false;

		public bool Prepared {
			get { return prepared; }
		}

		ImageOrientation pixbuf_orientation = ImageOrientation.TopLeft;

		public ImageOrientation PixbufOrientation {
			get { return pixbuf_orientation; }
		}

		bool is_disposed = false;

		public override void Dispose ()
		{
			is_disposed = true;
			if (image_stream != null) {
				try {
					image_stream.Close ();
				} catch (GLib.GException) {
				}
			}
			Close ();
			if (thumb != null) {
				thumb.Dispose ();
				thumb = null;
			}
			base.Dispose ();
		}

		public new bool Close ()
		{
			lock (sync_handle) {
				try {
					return base.Close ();
				}
				catch (GLib.GException) {
					return false;
				}
			}
		}
#endregion

#region event handlers
		protected override void OnAreaPrepared ()
		{
			if (is_disposed)
				return;

			prepared = notify_prepared = true;
			damage = Rectangle.Zero;
			base.OnAreaPrepared ();
		}

		protected override void OnAreaUpdated (int x, int y, int width, int height)
		{
			if (is_disposed)
				return;

			Rectangle area = new Rectangle (x, y, width, height);
			damage = damage == Rectangle.Zero ? area : damage.Union (area);
			base.OnAreaUpdated (x, y, width, height);
		}

		protected virtual void OnCompleted ()
		{
			if (is_disposed)
				return;

			EventHandler eh = Completed;
			if (eh != null) {
				eh (this, EventArgs.Empty);
			}
			Close ();
		}
#endregion

#region private stuffs
		System.IO.Stream image_stream;
		const int count = 1 << 16;
		byte[] buffer = new byte [count];
		bool notify_completed = false;
		Rectangle damage;
		object sync_handle = new object ();

		void HandleReadDone (IAsyncResult ar)
		{
			if (is_disposed)
				return;

			int byte_read = image_stream.EndRead (ar);
			lock (sync_handle) {
				if (byte_read == 0) {
					image_stream.Close ();
					Close ();
					loading = false;
					notify_completed = true;
				} else {
					try {
						if (!is_disposed && Write (buffer, (ulong)byte_read)) {
							image_stream.BeginRead (buffer, 0, count, HandleReadDone, null);
						}
					} catch (System.ObjectDisposedException) {
					} catch (GLib.GException) {
					}
				}
			}

			GLib.Idle.Add (delegate {
				//Send the AreaPrepared event
				if (notify_prepared) {
					notify_prepared = false;
					if (thumb != null) {
						thumb.Dispose ();
						thumb = null;
					}

					EventHandler<AreaPreparedEventArgs> eh = AreaPrepared;
					if (eh != null) {
						eh (this, new AreaPreparedEventArgs (false));
					}
				}

				//Send the AreaUpdated events
				if (damage != Rectangle.Zero) {
					EventHandler<AreaUpdatedEventArgs> eh = AreaUpdated;
					if (eh != null) {
						eh (this, new AreaUpdatedEventArgs (damage));
					}
					damage = Rectangle.Zero;
				}

				//Send the Completed event
				if (notify_completed) {
					notify_completed = false;
					OnCompleted ();
				}

				return false;
			});
		}
#endregion
	}
}
