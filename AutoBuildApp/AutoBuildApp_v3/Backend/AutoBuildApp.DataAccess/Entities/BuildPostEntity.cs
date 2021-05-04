using AutoBuildApp.Models.Reflections;
using AutoBuildApp.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Models.Entities
{
    /// <summary>
    /// This class contains the entity objects that will be
    /// transferred to the DAO for DB interactions.
    /// </summary>
    public class BuildPostEntity
    {
        // This string is the unique identifier of the build post.
        public string EntityId { get; set; }

        // The username of a registered user.
        public string Username { [DAO("username", typeof(string), false)] get; set; }

        // This string stores the name of the build that a user sets it.
        public string Title { [DAO("title", typeof(string), false)] get; set; }

        // The description of a certain build post.
        public string Description { [DAO("description", typeof(string), false)] get; set; }

        // This var will send an integer to the DB and initialize the counter
        // to 0 for a newly created build post.
        public int LikeIncrementor { [DAO("likes", typeof(string), false)] get; set; }

        // This int will specify what kind of build is being posted as an int value. 
        public int BuildTypeValue { [DAO("buildtype", typeof(string), false)] get; set; }

        // This string will mark the destination of the image that has been
        // downloaded from the front end from the user.
        public string BuildImagePath { [DAO("imagepath", typeof(string), false)] get; set; }

        // This string will be the time that the post was submitted.
        public string DateTime { [DAO("datetime", typeof(string), false)] get; set; }
    }
}
