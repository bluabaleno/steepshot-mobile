using Android.App;
using Android.OS;
using Com.Bumptech.Glide;
using Com.Lilarcor.Cheeseknife;
using Steepshot.Base;
using Steepshot.Utils;

namespace Steepshot.Activity
{
    [Activity(Label = "PostPreviewActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public sealed class PostPreviewActivity : BaseActivity
    {
        public const string PhotoExtraPath = "PhotoExtraPath";
        private string path;

#pragma warning disable 0649, 4014
        [InjectView(Resource.Id.photo)] private ScaleImageView _photo;
#pragma warning restore 0649


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.lyt_post_preview);
            Cheeseknife.Inject(this);

            path = Intent.GetStringExtra(PhotoExtraPath);
            if (!string.IsNullOrWhiteSpace(path))
                Glide.With(this).Load(path).Into(_photo);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Cheeseknife.Reset(this);
        }
    }
}
