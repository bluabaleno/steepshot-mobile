﻿using System;
using System.IO;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using Steepshot.Core;
using Steepshot.Core.Authority;
using Sweetshot.Library.HttpClient;
using Sweetshot.Library.Models.Common;
using Sweetshot.Library.Models.Requests;

namespace Sweetshot.Tests
{
    [TestFixture]
    public class IntegrationTestsChangingState
    {
        private readonly ISteepshotApiClient _steem = new SteepshotApiClient(KnownChains.Steem, false);
        private readonly ISteepshotApiClient _golos = new SteepshotApiClient(KnownChains.Golos, false);
        private ISteepshotApiClient Api(string name)
        {
            switch (name)
            {
                case "Steem":
                    return _steem;
                case "Golos":
                    return _golos;
                default:
                    return null;
            }
        }

        private UserInfo Authenticate(string name, string postingKey, ISteepshotApiClient api)
        {
            var request = new LoginWithPostingKeyRequest(name, postingKey);
            var response = api.LoginWithPostingKey(request).Result;
            return new UserInfo() { Login = name, PostingKey = postingKey, SessionId = response.Result.SessionId };
        }

        [Test, Sequential]
        public void BlockchainStateChangingTest([Values("Steem", "Golos")] string apiName, [Values("asduj", "pmartynov")] string user)
        {
            const string Name = "joseph.kalu";
            const string PostingKey = "***REMOVED***";

            var userInfo = Authenticate(Name, PostingKey, Api(apiName));

            // 1) Create new post
            var dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            var path = Path.Combine(dir.Parent.Parent.FullName, @"Data/cat.jpg");
            var file = File.ReadAllBytes(path);

            var createPostRequest = new UploadImageRequest(userInfo, "cat" + DateTime.UtcNow.Ticks, file, "cat1", "cat2", "cat3", "cat4");
            var createPostResponse = Api(apiName).Upload(createPostRequest).Result;

            AssertResult(createPostResponse);
            Assert.That(createPostResponse.Result.Body, Is.Not.Empty);
            Assert.That(createPostResponse.Result.Title, Is.Not.Empty);
            Assert.That(createPostResponse.Result.Tags, Is.Not.Empty);

            // Wait for data to be writed into blockchain
            Thread.Sleep(TimeSpan.FromSeconds(15));

            // Load last created post
            var userPostsRequest = new UserPostsRequest(Name);
            var userPostsResponse = Api(apiName).GetUserPosts(userPostsRequest).Result;
            AssertResult(userPostsResponse);
            var lastPost = userPostsResponse.Result.Results.First();
            Assert.That(createPostResponse.Result.Title, Is.EqualTo(lastPost.Title));

            // 2) Create new comment
            // Wait for 20 seconds before commenting
            Thread.Sleep(TimeSpan.FromSeconds(20));
            const string body = "Ллойс!";
            const string title = "Лучший камент ever";
            var createCommentRequest = new CreateCommentRequest(userInfo, lastPost.Url, body, title);
            var createCommentResponse = Api(apiName).CreateComment(createCommentRequest).Result;
            AssertResult(createCommentResponse);
            Assert.That(createCommentResponse.Result.IsCreated, Is.True);
            Assert.That(createCommentResponse.Result.Message, Is.EqualTo("Comment created"));

            // Wait for data to be writed into blockchain
            Thread.Sleep(TimeSpan.FromSeconds(15));

            // Load comments for this post and check them
            var getCommentsRequest = new GetCommentsRequest(lastPost.Url);
            var commentsResponse = Api(apiName).GetComments(getCommentsRequest).Result;
            AssertResult(commentsResponse);
            Assert.That(commentsResponse.Result.Results.First().Title, Is.EqualTo(title));
            Assert.That(commentsResponse.Result.Results.First().Body, Is.EqualTo(body));

            // 3) Vote up
            var voteUpRequest = new VoteRequest(userInfo, true, lastPost.Url);
            var voteUpResponse = Api(apiName).Vote(voteUpRequest).Result;
            AssertResult(voteUpResponse);
            Assert.That(voteUpResponse.Result.IsVoted, Is.True);
            Assert.That(voteUpResponse.Result.NewTotalPayoutReward, Is.Not.Null);
            Assert.That(voteUpResponse.Result.Message, Is.EqualTo("Upvoted"));
            Assert.That(voteUpResponse.Result.NewTotalPayoutReward, Is.Not.Null);

            // Wait for data to be writed into blockchain
            Thread.Sleep(TimeSpan.FromSeconds(15));
            // Provide sessionId with request to be able read voting information
            userPostsRequest.SessionId = userInfo.SessionId;
            var userPostsResponse2 = Api(apiName).GetUserPosts(userPostsRequest).Result;
            // Check if last post was voted
            AssertResult(userPostsResponse2);
            Assert.That(userPostsResponse2.Result.Results.First().Vote, Is.True);

            // 4) Vote down
            var voteDownRequest = new VoteRequest(userInfo, false, lastPost.Url);
            var voteDownResponse = Api(apiName).Vote(voteDownRequest).Result;
            AssertResult(voteDownResponse);
            Assert.That(voteDownResponse.Result.IsVoted, Is.False);
            Assert.That(voteDownResponse.Result.NewTotalPayoutReward, Is.Not.Null);
            Assert.That(voteDownResponse.Result.Message, Is.EqualTo("Downvoted"));
            Assert.That(voteDownResponse.Result.NewTotalPayoutReward, Is.Not.Null);

            // Wait for data to be writed into blockchain
            Thread.Sleep(TimeSpan.FromSeconds(15));
            // Provide sessionId with request to be able read voting information
            userPostsRequest.SessionId = userInfo.SessionId;
            var userPostsResponse3 = Api(apiName).GetUserPosts(userPostsRequest).Result;
            // Check if last post was voted
            AssertResult(userPostsResponse3);
            Assert.That(userPostsResponse3.Result.Results.First().Vote, Is.False);

            // 5) Vote up comment
            var commentUrl = commentsResponse.Result.Results.First().Url.Split('#').Last();
            var voteUpCommentRequest = new VoteRequest(userInfo, true, commentUrl);
            var voteUpCommentResponse = Api(apiName).Vote(voteUpCommentRequest).Result;
            AssertResult(voteUpCommentResponse);
            Assert.That(voteUpCommentResponse.Result.IsVoted, Is.True);
            Assert.That(voteUpCommentResponse.Result.NewTotalPayoutReward, Is.Not.Null);
            Assert.That(voteUpCommentResponse.Result.Message, Is.EqualTo("Upvoted"));
            Assert.That(voteUpCommentResponse.Result.NewTotalPayoutReward, Is.Not.Null);

            // Wait for data to be writed into blockchain
            Thread.Sleep(TimeSpan.FromSeconds(15));
            // Provide sessionId with request to be able read voting information
            getCommentsRequest.SessionId = userInfo.SessionId;
            var commentsResponse2 = Api(apiName).GetComments(getCommentsRequest).Result;
            // Check if last comment was voted
            AssertResult(commentsResponse2);
            Assert.That(commentsResponse2.Result.Results.First().Vote, Is.True);

            // 6) Vote down comment
            var voteDownCommentRequest = new VoteRequest(userInfo, false, commentUrl);
            var voteDownCommentResponse = Api(apiName).Vote(voteDownCommentRequest).Result;
            AssertResult(voteDownCommentResponse);
            Assert.That(voteDownCommentResponse.Result.IsVoted, Is.False);
            Assert.That(voteDownCommentResponse.Result.NewTotalPayoutReward, Is.Not.Null);
            Assert.That(voteDownCommentResponse.Result.Message, Is.EqualTo("Downvoted"));
            Assert.That(voteDownCommentResponse.Result.NewTotalPayoutReward, Is.Not.Null);

            // Wait for data to be writed into blockchain
            Thread.Sleep(TimeSpan.FromSeconds(15));
            // Provide sessionId with request to be able read voting information
            getCommentsRequest.SessionId = userInfo.SessionId;
            var commentsResponse3 = Api(apiName).GetComments(getCommentsRequest).Result;
            // Check if last comment was voted
            AssertResult(commentsResponse3);
            Assert.That(commentsResponse3.Result.Results.First().Vote, Is.False);

            // 7) Follow
            // Wait for data to be writed into blockchain
            Thread.Sleep(TimeSpan.FromSeconds(15));
            var followRequest = new FollowRequest(userInfo, FollowType.Follow, user);
            var followResponse = Api(apiName).Follow(followRequest).Result;
            AssertResult(followResponse);
            Assert.That(followResponse.Result.IsFollowed, Is.True);
            Assert.That(followResponse.Result.Message, Is.EqualTo("User is followed"));

            // 8) UnFollow
            // Wait for data to be writed into blockchain
            Thread.Sleep(TimeSpan.FromSeconds(15));
            var unfollowRequest = new FollowRequest(userInfo, FollowType.UnFollow, user);
            var unfollowResponse = Api(apiName).Follow(unfollowRequest).Result;
            AssertResult(unfollowResponse);
            Assert.That(unfollowResponse.Result.IsFollowed, Is.False);
            Assert.That(unfollowResponse.Result.Message, Is.EqualTo("User is unfollowed"));

            // 9) Logout
            var logoutRequest = new LogoutRequest(userInfo);
            var logoutResponse = Api(apiName).Logout(logoutRequest).Result;
            AssertResult(logoutResponse);
            Assert.That(logoutResponse.Result.IsLoggedOut, Is.True);
            Assert.That(logoutResponse.Result.Message, Is.EqualTo("User is logged out"));
        }

        //[Ignore("Ingoring...")]
        //public void Upload_Throttling()
        //{
        //// Arrange
        //var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\cat.jpg");
        //var file = File.ReadAllBytes(path);
        //var request = new UploadImageRequest(_sessionId, "cat" + DateTime.UtcNow.Ticks, file, "cat1", "cat2", "cat3", "cat4");

        //// Act
        //var response = _api.Upload(request).Result;
        //var response2 = _api.Upload(request).Result;
        //var response3 = _api.Upload(request).Result;

        //// Assert
        //AssertFailedResult(response3);
        //Assert.That(response3.Errors.Contains("Creating post is impossible. Please try 10 minutes later."));
        //}

        private void AssertResult<T>(OperationResult<T> response)
        {
            Assert.That(response, Is.Not.Null);

            if (response.Success)
            {
                Assert.That(response.Result, Is.Not.Null);
                Assert.That(response.Errors, Is.Empty);
            }
            else
            {
                Assert.That(response.Result, Is.Null);
                Assert.That(response.Errors, Is.Not.Empty);

                foreach (var error in response.Errors)
                {
                    Console.WriteLine(error);
                }
            }
        }
    }
}