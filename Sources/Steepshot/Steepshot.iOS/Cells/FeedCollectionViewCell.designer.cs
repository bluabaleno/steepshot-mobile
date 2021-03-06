// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Steepshot.iOS.Cells
{
    [Register ("FeedCollectionViewCell")]
    partial class FeedCollectionViewCell
    {
        [Outlet]
        UIKit.UIStackView actionStackView { get; set; }


        [Outlet]
        UIKit.UIImageView avatarImage { get; set; }


        [Outlet]
        UIKit.UIImageView bodyImage { get; set; }


        [Outlet]
        UIKit.UILabel cellText { get; set; }


        [Outlet]
        UIKit.UILabel commentText { get; set; }


        [Outlet]
        UIKit.UIView commentView { get; set; }


        [Outlet]
        UIKit.UIView contentView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint contentViewWidth { get; set; }


        [Outlet]
        UIKit.UIImageView firstLiker { get; set; }


        [Outlet]
        UIKit.UIButton flagButton { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint imageHeight { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint imageWidth { get; set; }


        [Outlet]
        UIKit.UIButton likeButton { get; set; }


        [Outlet]
        UIKit.UILabel netVotes { get; set; }


        [Outlet]
        UIKit.UILabel postTimeStamp { get; set; }


        [Outlet]
        UIKit.UILabel rewards { get; set; }


        [Outlet]
        UIKit.UIImageView secondLiker { get; set; }


        [Outlet]
        UIKit.UIImageView thirdLiker { get; set; }


        [Outlet]
        UIKit.UIStackView topLikers { get; set; }


        [Outlet]
        UIKit.UILabel viewCommentText { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (actionStackView != null) {
                actionStackView.Dispose ();
                actionStackView = null;
            }

            if (avatarImage != null) {
                avatarImage.Dispose ();
                avatarImage = null;
            }

            if (bodyImage != null) {
                bodyImage.Dispose ();
                bodyImage = null;
            }

            if (cellText != null) {
                cellText.Dispose ();
                cellText = null;
            }

            if (commentView != null) {
                commentView.Dispose ();
                commentView = null;
            }

            if (contentView != null) {
                contentView.Dispose ();
                contentView = null;
            }

            if (contentViewWidth != null) {
                contentViewWidth.Dispose ();
                contentViewWidth = null;
            }

            if (firstLiker != null) {
                firstLiker.Dispose ();
                firstLiker = null;
            }

            if (flagButton != null) {
                flagButton.Dispose ();
                flagButton = null;
            }

            if (imageHeight != null) {
                imageHeight.Dispose ();
                imageHeight = null;
            }

            if (likeButton != null) {
                likeButton.Dispose ();
                likeButton = null;
            }

            if (netVotes != null) {
                netVotes.Dispose ();
                netVotes = null;
            }

            if (postTimeStamp != null) {
                postTimeStamp.Dispose ();
                postTimeStamp = null;
            }

            if (rewards != null) {
                rewards.Dispose ();
                rewards = null;
            }

            if (secondLiker != null) {
                secondLiker.Dispose ();
                secondLiker = null;
            }

            if (thirdLiker != null) {
                thirdLiker.Dispose ();
                thirdLiker = null;
            }

            if (topLikers != null) {
                topLikers.Dispose ();
                topLikers = null;
            }

            if (viewCommentText != null) {
                viewCommentText.Dispose ();
                viewCommentText = null;
            }
        }
    }
}