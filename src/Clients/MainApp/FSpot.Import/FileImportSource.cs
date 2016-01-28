//
// FileImportSource.cs
//
// Author:
//   Daniel Köb <daniel.koeb@peony.at>
//   Ruben Vermeersch <ruben@savanne.be>
//
// Copyright (C) 2014 Daniel Köb
// Copyright (C) 2010 Novell, Inc.
// Copyright (C) 2010 Ruben Vermeersch
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
using System.Collections.Generic;

using Hyena;

using FSpot.Core;
using FSpot.Utils;
using FSpot.Imaging;

using Gtk;

namespace FSpot.Import
{
	internal class FileImportSource : IImportSource
	{
		public string Name { get; set; }

		public string IconName { get; set; }

		public SafeUri Root { get; set; }

		public Thread PhotoScanner;
		bool run_photoscanner = false;

		public FileImportSource (SafeUri root, string name, string icon_name)
		{
			Root = root;
			Name = name;

			if (root != null) {
				if (IsIPodPhoto) {
					IconName = "multimedia-player";
				} else if (IsCamera) {
					IconName = "media-flash";
				} else {
					IconName = icon_name;
				}
			}
		}

		public void StartPhotoScan (ImportController controller, PhotoList photo_list)
		{
			if (PhotoScanner != null) {
				run_photoscanner = false;
				PhotoScanner.Join ();
			}

			run_photoscanner = true;
			PhotoScanner = ThreadAssist.Spawn (() => ScanPhotos (controller, photo_list));
		}

		protected virtual void ScanPhotos (ImportController controller, PhotoList photo_list)
		{
			ScanPhotoDirectory (controller, Root, photo_list);
			ThreadAssist.ProxyToMain (() => controller.PhotoScanFinished ());
		}

		protected void ScanPhotoDirectory (ImportController controller, SafeUri uri, PhotoList photo_list)
		{
			var enumerator = new RecursiveFileEnumerator (uri) {
						Recurse = controller.RecurseSubdirectories,
						CatchErrors = true,
						IgnoreSymlinks = true
			};
			foreach (var file in enumerator) {
				if (ImageFile.HasLoader (new SafeUri (file.Uri.ToString(), true))) {
					var info = new FileImportInfo (new SafeUri (file.Uri.ToString (), true));
					ThreadAssist.ProxyToMain (() =>
					    photo_list.Add (info));
				}
				if (!run_photoscanner)
					return;
			}
		}

		public void Deactivate ()
		{
			if (PhotoScanner != null) {
				run_photoscanner = false;
				PhotoScanner.Join ();

				// Make sure all photos are added. This is needed to prevent
				// a race condition where a source is deactivated, yet photos
				// are still added to the collection because they are
				// queued on the mainloop.
				while (Application.EventsPending ()) {
					Application.RunIteration (false);
				}

				PhotoScanner = null;
			}
		}

		private bool IsCamera {
			get {
				try {
					var file = GLib.FileFactory.NewForUri (Root.Append ("DCIM"));
					return file.Exists;
				} catch {
					return false;
				}
			}
		}

		private bool IsIPodPhoto {
			get {
				try {
					var file = GLib.FileFactory.NewForUri (Root.Append ("Photos"));
					var file2 = GLib.FileFactory.NewForUri (Root.Append ("iPod_Control"));
					return file.Exists && file2.Exists;
				} catch {
					return false;
				}
			}
		}
	}

	// Multi root version for drag and drop import.
	internal class MultiFileImportSource : FileImportSource
	{
		private IEnumerable<SafeUri> uris;

		public MultiFileImportSource (IEnumerable<SafeUri> uris)
			: base (null, String.Empty, String.Empty)
		{
			this.uris = uris;
		}

		protected override void ScanPhotos (ImportController controller, PhotoList photo_list)
		{
			foreach (var uri in uris) {
				Log.Debug ("Scanning " + uri);
				ScanPhotoDirectory (controller, uri, photo_list);
			}
			ThreadAssist.ProxyToMain (() => controller.PhotoScanFinished ());
		}
	}

	internal class FileImportInfo : FilePhoto
	{
		public FileImportInfo (SafeUri original) : base (original)
		{
		}

		public SafeUri DestinationUri { get; set; }

		internal uint PhotoId { get; set; }
	}
}
