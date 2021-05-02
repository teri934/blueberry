using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using DD = Dictionary.Database;

namespace Dictionary.Implemented
{
	class ResultAdapter : BaseAdapter
	{
		public List<DD.Result> data;
		private static LayoutInflater inflater;
		Android.Content.Context context;

		public ResultAdapter(Android.Content.Context context, List<DD.Result> data)
		{
			this.data = data;
			this.context = context;
			inflater = (LayoutInflater)context.GetSystemService(Android.Content.Context.LayoutInflaterService);
		}
		public override int Count => data.Count;

		public override Java.Lang.Object GetItem(int position) => data[position];

		public override long GetItemId(int position) => position;

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View v = convertView;
			if (convertView == null)
				v = inflater.Inflate(Dictionary.Resource.Layout.result_line, null);

			TextView score = (TextView)v.FindViewById(Dictionary.Resource.Id.score);
			score.Text = data[position].Score.ToString();

			TextView rounds = (TextView)v.FindViewById(Dictionary.Resource.Id.rounds);
			rounds.Text = data[position].Rounds.ToString();

			return v;
		}
	}
}