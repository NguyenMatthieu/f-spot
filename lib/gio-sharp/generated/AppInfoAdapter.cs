// This file was generated by the Gtk# code generator.
// Any changes made will be lost if regenerated.

namespace GLib {

	using System;
	using System.Runtime.InteropServices;

#region Autogenerated code
	public class AppInfoAdapter : GLib.GInterfaceAdapter, GLib.AppInfo {

		public AppInfoAdapter (IntPtr handle)
		{
			this.handle = handle;
		}

		[DllImport("libgio-2.0-0.dll")]
		static extern IntPtr g_app_info_get_type();

		private static GLib.GType _gtype = new GLib.GType (g_app_info_get_type ());

		public override GLib.GType GType {
			get {
				return _gtype;
			}
		}

		IntPtr handle;
		public override IntPtr Handle {
			get {
				return handle;
			}
		}

		public static AppInfo GetObject (IntPtr handle, bool owned)
		{
			GLib.Object obj = GLib.Object.GetObject (handle, owned);
			return GetObject (obj);
		}

		public static AppInfo GetObject (GLib.Object obj)
		{
			if (obj == null)
				return null;
			else if (obj as AppInfo == null)
				return new AppInfoAdapter (obj.Handle);
			else
				return obj as AppInfo;
		}

		[DllImport("libgio-2.0-0.dll")]
		static extern IntPtr g_app_info_get_executable(IntPtr raw);

		public string Executable { 
			get {
				IntPtr raw_ret = g_app_info_get_executable(Handle);
				string ret = GLib.Marshaller.Utf8PtrToString (raw_ret);
				return ret;
			}
		}

		[DllImport("libgio-2.0-0.dll")]
		static extern IntPtr g_app_info_get_name(IntPtr raw);

		public string Name { 
			get {
				IntPtr raw_ret = g_app_info_get_name(Handle);
				string ret = GLib.Marshaller.Utf8PtrToString (raw_ret);
				return ret;
			}
		}

		[DllImport("libgio-2.0-0.dll")]
		static extern IntPtr g_app_info_dup(IntPtr raw);

		public GLib.AppInfo Dup() {
			IntPtr raw_ret = g_app_info_dup(Handle);
			GLib.AppInfo ret = GLib.AppInfoAdapter.GetObject (raw_ret, false);
			return ret;
		}

		[DllImport("libgio-2.0-0.dll")]
		static extern bool g_app_info_equal(IntPtr raw, IntPtr appinfo2);

		public bool Equal(GLib.AppInfo appinfo2) {
			bool raw_ret = g_app_info_equal(Handle, appinfo2 == null ? IntPtr.Zero : appinfo2.Handle);
			bool ret = raw_ret;
			return ret;
		}

		[DllImport("libgio-2.0-0.dll")]
		static extern void g_app_info_reset_type_associations(IntPtr content_type);

		public static void ResetTypeAssociations(string content_type) {
			IntPtr native_content_type = GLib.Marshaller.StringToPtrGStrdup (content_type);
			g_app_info_reset_type_associations(native_content_type);
			GLib.Marshaller.Free (native_content_type);
		}

		[DllImport("libgio-2.0-0.dll")]
		static extern bool g_app_info_set_as_default_for_type(IntPtr raw, IntPtr content_type, out IntPtr error);

		public bool SetAsDefaultForType(string content_type) {
			IntPtr native_content_type = GLib.Marshaller.StringToPtrGStrdup (content_type);
			IntPtr error = IntPtr.Zero;
			bool raw_ret = g_app_info_set_as_default_for_type(Handle, native_content_type, out error);
			bool ret = raw_ret;
			GLib.Marshaller.Free (native_content_type);
			if (error != IntPtr.Zero) throw new GLib.GException (error);
			return ret;
		}

		[DllImport("libgio-2.0-0.dll")]
		static extern IntPtr g_app_info_get_description(IntPtr raw);

		public string Description { 
			get {
				IntPtr raw_ret = g_app_info_get_description(Handle);
				string ret = GLib.Marshaller.Utf8PtrToString (raw_ret);
				return ret;
			}
		}

		[DllImport("libgio-2.0-0.dll")]
		static extern IntPtr g_app_info_create_from_commandline(IntPtr commandline, IntPtr application_name, int flags, out IntPtr error);

		public static GLib.AppInfo CreateFromCommandline(string commandline, string application_name, GLib.AppInfoCreateFlags flags) {
			IntPtr native_commandline = GLib.Marshaller.StringToPtrGStrdup (commandline);
			IntPtr native_application_name = GLib.Marshaller.StringToPtrGStrdup (application_name);
			IntPtr error = IntPtr.Zero;
			IntPtr raw_ret = g_app_info_create_from_commandline(native_commandline, native_application_name, (int) flags, out error);
			GLib.AppInfo ret = GLib.AppInfoAdapter.GetObject (raw_ret, false);
			GLib.Marshaller.Free (native_commandline);
			GLib.Marshaller.Free (native_application_name);
			if (error != IntPtr.Zero) throw new GLib.GException (error);
			return ret;
		}

		[DllImport("libgio-2.0-0.dll")]
		static extern bool g_app_info_set_as_default_for_extension(IntPtr raw, IntPtr extension, out IntPtr error);

		public bool SetAsDefaultForExtension(string extension) {
			IntPtr native_extension = GLib.Marshaller.StringToPtrGStrdup (extension);
			IntPtr error = IntPtr.Zero;
			bool raw_ret = g_app_info_set_as_default_for_extension(Handle, native_extension, out error);
			bool ret = raw_ret;
			GLib.Marshaller.Free (native_extension);
			if (error != IntPtr.Zero) throw new GLib.GException (error);
			return ret;
		}

		[DllImport("libgio-2.0-0.dll")]
		static extern bool g_app_info_remove_supports_type(IntPtr raw, IntPtr content_type, out IntPtr error);

		public bool RemoveSupportsType(string content_type) {
			IntPtr native_content_type = GLib.Marshaller.StringToPtrGStrdup (content_type);
			IntPtr error = IntPtr.Zero;
			bool raw_ret = g_app_info_remove_supports_type(Handle, native_content_type, out error);
			bool ret = raw_ret;
			GLib.Marshaller.Free (native_content_type);
			if (error != IntPtr.Zero) throw new GLib.GException (error);
			return ret;
		}

		[DllImport("libgio-2.0-0.dll")]
		static extern bool g_app_info_launch_uris(IntPtr raw, IntPtr uris, IntPtr launch_context, out IntPtr error);

		public bool LaunchUris(GLib.List uris, GLib.AppLaunchContext launch_context) {
			IntPtr error = IntPtr.Zero;
			bool raw_ret = g_app_info_launch_uris(Handle, uris == null ? IntPtr.Zero : uris.Handle, launch_context == null ? IntPtr.Zero : launch_context.Handle, out error);
			bool ret = raw_ret;
			if (error != IntPtr.Zero) throw new GLib.GException (error);
			return ret;
		}

		[DllImport("libgio-2.0-0.dll")]
		static extern bool g_app_info_launch(IntPtr raw, IntPtr files, IntPtr launch_context, out IntPtr error);

		public bool Launch(GLib.List files, GLib.AppLaunchContext launch_context) {
			IntPtr error = IntPtr.Zero;
			bool raw_ret = g_app_info_launch(Handle, files == null ? IntPtr.Zero : files.Handle, launch_context == null ? IntPtr.Zero : launch_context.Handle, out error);
			bool ret = raw_ret;
			if (error != IntPtr.Zero) throw new GLib.GException (error);
			return ret;
		}

		[DllImport("libgio-2.0-0.dll")]
		static extern bool g_app_info_launch_default_for_uri(IntPtr uri, IntPtr launch_context, out IntPtr error);

		public static bool LaunchDefaultForUri(string uri, GLib.AppLaunchContext launch_context) {
			IntPtr native_uri = GLib.Marshaller.StringToPtrGStrdup (uri);
			IntPtr error = IntPtr.Zero;
			bool raw_ret = g_app_info_launch_default_for_uri(native_uri, launch_context == null ? IntPtr.Zero : launch_context.Handle, out error);
			bool ret = raw_ret;
			GLib.Marshaller.Free (native_uri);
			if (error != IntPtr.Zero) throw new GLib.GException (error);
			return ret;
		}

		[DllImport("libgio-2.0-0.dll")]
		static extern IntPtr g_app_info_get_commandline(IntPtr raw);

		public string Commandline { 
			get {
				IntPtr raw_ret = g_app_info_get_commandline(Handle);
				string ret = GLib.Marshaller.Utf8PtrToString (raw_ret);
				return ret;
			}
		}

		[DllImport("libgio-2.0-0.dll")]
		static extern bool g_app_info_delete(IntPtr raw);

		public bool Delete() {
			bool raw_ret = g_app_info_delete(Handle);
			bool ret = raw_ret;
			return ret;
		}

		[DllImport("libgio-2.0-0.dll")]
		static extern IntPtr g_app_info_get_id(IntPtr raw);

		public string Id { 
			get {
				IntPtr raw_ret = g_app_info_get_id(Handle);
				string ret = GLib.Marshaller.Utf8PtrToString (raw_ret);
				return ret;
			}
		}

		[DllImport("libgio-2.0-0.dll")]
		static extern IntPtr g_app_info_get_icon(IntPtr raw);

		public GLib.Icon Icon { 
			get {
				IntPtr raw_ret = g_app_info_get_icon(Handle);
				GLib.Icon ret = GLib.IconAdapter.GetObject (raw_ret, false);
				return ret;
			}
		}

		[DllImport("libgio-2.0-0.dll")]
		static extern bool g_app_info_can_delete(IntPtr raw);

		public bool CanDelete() {
			bool raw_ret = g_app_info_can_delete(Handle);
			bool ret = raw_ret;
			return ret;
		}

		[DllImport("libgio-2.0-0.dll")]
		static extern bool g_app_info_should_show(IntPtr raw);

		public bool ShouldShow { 
			get {
				bool raw_ret = g_app_info_should_show(Handle);
				bool ret = raw_ret;
				return ret;
			}
		}

		[DllImport("libgio-2.0-0.dll")]
		static extern bool g_app_info_can_remove_supports_type(IntPtr raw);

		public bool CanRemoveSupportsType { 
			get {
				bool raw_ret = g_app_info_can_remove_supports_type(Handle);
				bool ret = raw_ret;
				return ret;
			}
		}

		[DllImport("libgio-2.0-0.dll")]
		static extern bool g_app_info_add_supports_type(IntPtr raw, IntPtr content_type, out IntPtr error);

		public bool AddSupportsType(string content_type) {
			IntPtr native_content_type = GLib.Marshaller.StringToPtrGStrdup (content_type);
			IntPtr error = IntPtr.Zero;
			bool raw_ret = g_app_info_add_supports_type(Handle, native_content_type, out error);
			bool ret = raw_ret;
			GLib.Marshaller.Free (native_content_type);
			if (error != IntPtr.Zero) throw new GLib.GException (error);
			return ret;
		}

		[DllImport("libgio-2.0-0.dll")]
		static extern bool g_app_info_supports_uris(IntPtr raw);

		public bool SupportsUris { 
			get {
				bool raw_ret = g_app_info_supports_uris(Handle);
				bool ret = raw_ret;
				return ret;
			}
		}

		[DllImport("libgio-2.0-0.dll")]
		static extern IntPtr g_app_info_get_default_for_uri_scheme(IntPtr uri_scheme);

		public static GLib.AppInfo GetDefaultForUriScheme(string uri_scheme) {
			IntPtr native_uri_scheme = GLib.Marshaller.StringToPtrGStrdup (uri_scheme);
			IntPtr raw_ret = g_app_info_get_default_for_uri_scheme(native_uri_scheme);
			GLib.AppInfo ret = GLib.AppInfoAdapter.GetObject (raw_ret, false);
			GLib.Marshaller.Free (native_uri_scheme);
			return ret;
		}

		[DllImport("libgio-2.0-0.dll")]
		static extern bool g_app_info_supports_files(IntPtr raw);

		public bool SupportsFiles { 
			get {
				bool raw_ret = g_app_info_supports_files(Handle);
				bool ret = raw_ret;
				return ret;
			}
		}

		[DllImport("libgio-2.0-0.dll")]
		static extern IntPtr g_app_info_get_default_for_type(IntPtr content_type, bool must_support_uris);

		public static GLib.AppInfo GetDefaultForType(string content_type, bool must_support_uris) {
			IntPtr native_content_type = GLib.Marshaller.StringToPtrGStrdup (content_type);
			IntPtr raw_ret = g_app_info_get_default_for_type(native_content_type, must_support_uris);
			GLib.AppInfo ret = GLib.AppInfoAdapter.GetObject (raw_ret, false);
			GLib.Marshaller.Free (native_content_type);
			return ret;
		}

		[DllImport("libgio-2.0-0.dll")]
		static extern IntPtr g_app_info_get_all_for_type(IntPtr content_type);

		public static GLib.AppInfo[] GetAllForType(string content_type) {
			IntPtr native_content_type = GLib.Marshaller.StringToPtrGStrdup (content_type);
			IntPtr raw_ret = g_app_info_get_all_for_type(native_content_type);
			GLib.AppInfo[] ret = (GLib.AppInfo[]) GLib.Marshaller.ListPtrToArray (raw_ret, typeof(GLib.List), true, false, typeof(GLib.AppInfo));
			GLib.Marshaller.Free (native_content_type);
			return ret;
		}

#endregion
#region Customized extensions
#line 1 "AppInfoAdapter.custom"
//
// AppInfoAdapter.custom
//
// Author:
//   Stephane Delcroix  <stephane@delcroix.org>
//
// Copyright (C) 2008 Novell, Inc.
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
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

		[DllImport("libgio-2.0-0.dll")]
		static extern IntPtr g_app_info_get_all();

		public static GLib.AppInfo[] GetAll() {
			IntPtr raw_ret = g_app_info_get_all();
			GLib.AppInfo[] ret = (GLib.AppInfo[]) GLib.Marshaller.ListPtrToArray (raw_ret, typeof(GLib.List), true, false, typeof(GLib.AppInfo));
			return ret;
		}

#endregion
	}
}
