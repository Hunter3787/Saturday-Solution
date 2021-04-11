using AutoBuildApp.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.DomainModels
{
    /// <summary>
    /// This class will be the domain model of a build post. 
    /// All published buids will contail these values.
    /// </summary>
    public class BuildPost
    {
        // This string is the unique identifier of the build post.
        public string EntityId { get; set; }

        // The username of a registered user.
        public string Username { get; set; }

        // This string stores the name of the build that a user sets it.
        public string Title { get; set; }

        // The description of a certain build post.
        public string Description { get; set; }

        // This var will send an integer to the DB and initialize the counter
        // to 0 for a newly created build post.
        public int LikeIncrementor { get; set; }

        // This enum will specify what king of build is being posted. 
        public BuildType BuildType { get; set; }

        // This string will mark the destination of the image that has been
        // downloaded from the front end from the user.
        public string BuildImagePath { get; set; }

        // This string will be the time that the post was submitted.
        public string DateTime { get; set; }
    }
}
