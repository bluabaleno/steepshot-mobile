using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Refractored.Controls;
using Square.Picasso;
using Steepshot.Core;
using Steepshot.Core.Models.Responses;
using Steepshot.Core.Presenters;
using Steepshot.Utils;

namespace Steepshot.Adapter
{
    public class FollowersAdapter : RecyclerView.Adapter
    {
        private readonly List<UserFriend> _collection;
        private readonly Context _context;
        public Action<int> FollowAction;
        public Action<int> UserAction;
        private Typeface[] _fonts;
        private FollowersPresenter _presenter;

        public FollowersAdapter(Context context, List<UserFriend> collection, FollowersPresenter presenter, Typeface[] fonts)
        {
            _context = context;
            _collection = collection;
            _presenter = presenter;
            _fonts = fonts;
        }

        public void InverseFollow(int pos)
        {
            _collection[pos].HasFollowed = !_collection[pos].HasFollowed;
        }

        public override int GetItemViewType(int position)
        {
            if(_collection.Count == position)
            {
                return (int)ViewType.Loader;
            }
            return (int)ViewType.Cell;
        }

        public UserFriend GetItem(int position)
        {
            return _collection[position];
        }

        public override int ItemCount => _collection.Count == 0 || !_presenter.HasItems ? _collection.Count : _collection.Count + 1;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var vh = holder as FollowersViewHolder;
            if (vh == null) return;

            var item = _collection[position];
            vh.FriendAvatar.SetImageResource(Resource.Drawable.ic_user_placeholder);
            vh.FriendName.Text = item.Author;
            vh.FriendLogin.Text = item.Author;
            if (!string.IsNullOrEmpty(item.Avatar))
                Picasso.With(_context).Load(item.Avatar).NoFade().Resize(150, 0).Into(vh.FriendAvatar);

            vh.UpdateData(item, _context);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            switch ((ViewType)viewType)
            {
                case ViewType.Loader:
                    var loaderView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.loading_item, parent, false);
                    var loaderVh = new LoaderViewHolder(loaderView);
                    return loaderVh;
                default:
                    var cellView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.lyt_followers_item, parent, false);
                    var cellVh = new FollowersViewHolder(cellView, FollowAction, UserAction, _context, _fonts);
                    return cellVh;
            }
        }

        public class LoaderViewHolder : RecyclerView.ViewHolder
        {
            public LoaderViewHolder(View itemView) : base(itemView)
            {
            }
        }

        private class FollowersViewHolder : RecyclerView.ViewHolder
        {
            public CircleImageView FriendAvatar { get; }
            public TextView FriendName { get; }
            public TextView FriendLogin { get; }
            private Button FollowButton { get; }

            private UserFriend _userFriendst;
            private readonly Action<int> _followAction;
            private readonly Action<int> _userAction;
            private Context _context;

            public FollowersViewHolder(View itemView, Action<int> followAction, Action<int> userAction, Context context, Typeface[] fonts)
                : base(itemView)
            {
                _context = context;
                FriendAvatar = itemView.FindViewById<CircleImageView>(Resource.Id.friend_avatar);
                FriendLogin = itemView.FindViewById<TextView>(Resource.Id.username);
                FriendName = itemView.FindViewById<TextView>(Resource.Id.name);
                FollowButton = itemView.FindViewById<Button>(Resource.Id.follow_button);

                FriendLogin.Typeface = fonts[1];
                FriendName.Typeface = fonts[0];

                _followAction = followAction;
                _userAction = userAction;
                FollowButton.Click += Follow_Click;
                FriendName.Clickable = true;
                FriendName.Click += User_Click;
                FriendAvatar.Clickable = true;
                FriendAvatar.Click += User_Click;
            }

            private void User_Click(object sender, EventArgs e)
            {
                _userAction?.Invoke(AdapterPosition);
            }

            void Follow_Click(object sender, EventArgs e)
            {
                _followAction?.Invoke(AdapterPosition);
                CheckFollow(this, !_userFriendst.HasFollowed, _context);
            }

            private void CheckFollow(FollowersViewHolder vh, bool follow, Context context)
            {
                if (BasePresenter.User.Login == vh.FriendLogin.Text)
                    vh.FollowButton.Visibility = ViewStates.Gone;
                else if (follow)
                {
                    var background = (GradientDrawable)vh.FollowButton.Background;
                    background.SetColor(Color.White);
                    background.SetStroke(1, BitmapUtils.GetColorFromInteger(ContextCompat.GetColor(context, Resource.Color.rgb244_244_246)));
                    vh.FollowButton.Text = Localization.Messages.Unfollow;
                    vh.FollowButton.SetTextColor(BitmapUtils.GetColorFromInteger(ContextCompat.GetColor(context, Resource.Color.rgb15_24_30)));
                }
                else
                {
                    var background = (GradientDrawable)vh.FollowButton.Background;
                    background.SetColor(BitmapUtils.GetColorFromInteger(ContextCompat.GetColor(context, Resource.Color.rgb231_72_0)));
                    background.SetStroke(0, Color.White);
                    vh.FollowButton.Text = Localization.Messages.Follow;
                    vh.FollowButton.SetTextColor(Color.White);
                }
            }

            public void UpdateData(UserFriend userFriendst, Context context)
            {
                _userFriendst = userFriendst;
                CheckFollow(this, _userFriendst.HasFollowed, context);
            }
        }
    }
}
