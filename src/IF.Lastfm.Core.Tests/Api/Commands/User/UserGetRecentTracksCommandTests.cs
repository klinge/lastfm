﻿using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Core.Tests.Resources;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.User;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Tests.Api.Commands
{
    public class UserGetRecentTracksCommandTests : CommandTestsBase
    {
        [Test]
        public async Task HandleResponseMultiple()
        {
            var command = new GetRecentTracksCommand(MAuth.Object, "rj")
            {
                Count = 1
            };

            var expectedTrack = new LastTrack
            {
                ArtistName = "The Who",
                TimePlayed = new DateTime(2014, 12, 19, 16, 13, 55,DateTimeKind.Utc),
                Mbid = "79f3dc97-2297-47ee-8556-9a1bb4b48d53",
                Name = "Pinball Wizard",
                ArtistMbid = "9fdaa16b-a6c4-4831-b87c-bc9ca8ce7eaa",
                AlbumName = "Tommy (Remastered)",
                Url = new Uri("http://www.last.fm/music/The+Who/_/Pinball+Wizard", UriKind.Absolute),
                Images = new LastImageSet(
                    "http://userserve-ak.last.fm/serve/34s/35234991.jpg",
                    "http://userserve-ak.last.fm/serve/64s/35234991.jpg",
                    "http://userserve-ak.last.fm/serve/126/35234991.jpg",
                    "http://userserve-ak.last.fm/serve/300x300/35234991.jpg")
            };

            var file = GetFileContents("UserApi.UserGetRecentTracksMultiple.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(UserApiResponses.UserGetRecentTracksMultiple));
            var actual = await command.HandleResponse(response);

            Assert.IsTrue(actual.Success);
            TestHelper.AssertSerialiseEqual(expectedTrack, actual.Content[2]);
        }

        [Test]
        public async Task HandleResponseSingle()
        {
            var command = new GetRecentTracksCommand(MAuth.Object, "rj")
            {
                Count = 1
            };

            var expectedTrack = new LastTrack
            {
                ArtistName = "Rick James",
                Mbid = "",
                Name = "Super Freak (Part 1) - 1981 Single Version",
                ArtistMbid = "cba9cec2-be8d-41bd-91b4-a1cd7de39b0c",

                TimePlayed = new DateTime(2014,12,20,10,16,52, DateTimeKind.Utc),
                AlbumName = "The Definitive Collection",
                Url = new Uri("http://www.last.fm/music/Rick+James/_/Super+Freak+(Part+1)+-+1981+Single+Version", UriKind.Absolute),
                Images = new LastImageSet(
                    "http://userserve-ak.last.fm/serve/34s/90462319.jpg",
                    "http://userserve-ak.last.fm/serve/64s/90462319.jpg",
                    "http://userserve-ak.last.fm/serve/126/90462319.jpg",
                    "http://userserve-ak.last.fm/serve/300x300/90462319.jpg")
            };
            
            var file = GetFileContents("UserApi.UserGetRecentTracksSingle.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(UserApiResponses.UserGetRecentTracksSingle));
            var actual = await command.HandleResponse(response);

            Assert.IsTrue(actual.Success);
            TestHelper.AssertSerialiseEqual(expectedTrack, actual.Single());
        }

        [Test]
        public async Task UserGetRecentTracks_HandleResponseNowPlaying_Success()
        {
            var command = new GetRecentTracksCommand(MAuth.Object, "rj")
            {
                Count = 1
            };
            
            var file = GetFileContents("UserApi.UserGetRecentTracksNowPlaying.json");
            var response = CreateResponseMessage(file);
            var actual = await command.HandleResponse(response);

            Assert.IsTrue(actual.Success);
            Assert.AreEqual(6, actual.Content.Count);
            Assert.AreEqual("Crosby, Stills, Nash & Young", actual.Content.ElementAt(0).ArtistName);
            Assert.IsTrue(actual.Content.ElementAt(0).IsNowPlaying);
            Assert.IsNull(actual.Content.ElementAt(1).IsNowPlaying);
        }

        [Test]
        public async Task HandleErrorResponse()
        {
            var command = new GetRecentTracksCommand(MAuth.Object, "rj")
            {
                Count = 1
            };

            var file = GetFileContents("UserApi.UserGetRecentTracksError.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(UserApiResponses.UserGetRecentTracksError));

            var parsed = await command.HandleResponse(response);

            Assert.IsFalse(parsed.Success);
            Assert.IsTrue(parsed.Status == LastResponseStatus.MissingParameters);
        }
    }
}
