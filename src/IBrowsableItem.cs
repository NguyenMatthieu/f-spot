namespace FSpot {
	public delegate void IBrowsableCollectionChangedHandler (IBrowsableCollection collection);
	public delegate void IBrowsableCollectionItemsChangedHandler (IBrowsableCollection collection, BrowsableArgs args);

	/*
	public interface IBrowsableSelection : IBrowsableCollection {
		int [] ParentPositions ();
		public void Clear ();
		public void SelectAll ();
	}
	*/

	public class BrowsableArgs : System.EventArgs {
		int [] items;

		public int [] Items {
			get { return items; }
		}

		public BrowsableArgs (int num)
		{
			items = new int [] { num };
		}

		public BrowsableArgs (int [] items)
		{
			this.items = items;
		}
	}

	public interface IBrowsableCollection {
		// FIXME this should really be ToArray ()
		IBrowsableItem [] Items {
			get;
		}
		
		int IndexOf (IBrowsableItem item);

		IBrowsableItem this [int index] {
			get;
		}

		int Count {
			get;
		}

		bool Contains (IBrowsableItem item);

		// FIXME the Changed event needs to pass along information
		// about the items that actually changed if possible.  For things like
		// TrayView everything has to be redrawn when a single
		// item has been added or removed which adds too much
		// overhead.
		event IBrowsableCollectionChangedHandler Changed;
		event IBrowsableCollectionItemsChangedHandler ItemsChanged;
	}

	public interface IBrowsableItem {
		System.DateTime Time {
			get;
		}
		
		Tag [] Tags {
			get;
		}

		System.Uri DefaultVersionUri {
			get;
		}

		string Description {
			get;
		}

		string Name {
			get; 
		}
	}

	public delegate void ItemIndexChangedHandler (BrowsablePointer pointer, IBrowsableItem old);

	public class BrowsablePointer {
		IBrowsableCollection collection;
		IBrowsableItem item;
		int index;
		public event ItemIndexChangedHandler IndexChanged;

		public BrowsablePointer (IBrowsableCollection collection, int index)
		{
			this.collection = collection;
			this.Index = index;
			item = Current;

			collection.Changed += HandleCollectionChanged;
		}

		public IBrowsableCollection Collection {
			get {
				return collection;
			}
		}

		public IBrowsableItem Current {
			get {
				if (!this.IsValid)
					return null;
				else 
					return collection [index];
			}
		}

		private bool Valid (int val)
		{
			return val >= 0 && val < collection.Count;
		}

		public bool IsValid {
			get {
				return Valid (this.Index);
			}
		}

		public void MoveFirst ()
		{
			Index = 0;
		}

		public void MoveLast ()
		{
			Index = collection.Count - 1;
		}
		
		public void MoveNext ()
		{
			MoveNext (false);
		}

		public void MoveNext (bool wrap)
		{
			int val = Index;

			val++;
			if (!Valid (val))
				val = wrap ? 0 : Index;
			
			Index = val;
		}
		
		public void MovePrevious ()
		{
			MovePrevious (false);
		}

		public void MovePrevious (bool wrap)
		{
			int val = Index;

			val--;
			if (!Valid (val))
				val = wrap ? collection.Count - 1 : Index;

			Index = val;
		}

		public int Index {
			get {
				return index;
			}
			set {
				if (index != value) {
					IBrowsableItem old = item;
					index = value;
					item = Current;
					if (IndexChanged != null)
						IndexChanged (this, old);
				}				
			}
		}


		protected virtual void HandleCollectionChanged (IBrowsableCollection collection)
		{
			int next_location = collection.IndexOf (item);
		        Index = Valid (next_location) ? next_location : 0;
		}
	}
}	
