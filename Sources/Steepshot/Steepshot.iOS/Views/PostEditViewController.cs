﻿using UIKit;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using FFImageLoading;
using CoreGraphics;
using PureLayout.Net;
using Steepshot.iOS.Cells;
using Steepshot.iOS.ViewSources;
using Steepshot.Core.Models.Common;
using Steepshot.Core.Models.Requests;
using Steepshot.Core.Presenters;
using Steepshot.Core.Utils;
using Steepshot.Core.Localization;
namespace Steepshot.iOS.Views
{
    public class PostEditViewController : DescriptionViewController
    {
        public PostEditViewController(Post editPost)
        {
            post = editPost;
            editMode = true;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupTags();
            SetupFields();

            model = new PreparePostModel(AppSettings.User.UserInfo, post, AppSettings.AppInfo.GetModel());
        }

        protected override void SetImage()
        {
            if (post.Media.Length > 1)
            {
                SetupPhotoCollection();

                List<string> URLs = new List<string>();

                foreach (var media in post.Media)
                    URLs.Add(media.Thumbnails.Micro);

                var galleryCollectionViewSource = new PhotoGalleryViewSource(URLs);
                photoCollection.Source = galleryCollectionViewSource;
            }
            else
            {
                SetupPhoto();

                var stringUrl = post.Media[0].Thumbnails.Mini;
                ImageService.Instance.LoadUrl(stringUrl).Into(photoView);
            }
        }

        private void SetupTags()
        {
            for (int i = 0; i < post.Tags.Length - 1; i++)
            {
                collectionviewSource.LocalTags.Add(post.Tags[i]);
                collectionViewDelegate.GenerateVariables();
            }

            tagsCollectionView.ReloadData();
            ResizeView();
        }

        private void SetupFields()
        {
            if (!string.IsNullOrEmpty(post.Title))
            {
                titleTextField.Text = post.Title;
                titlePlaceholderLabel.Hidden = true;
            }

            if (!string.IsNullOrEmpty(post.Description))
            {
                descriptionTextField.Text = post.Description;
                descriptionPlaceholderLabel.Hidden = true;
            }
        }
    }
}
