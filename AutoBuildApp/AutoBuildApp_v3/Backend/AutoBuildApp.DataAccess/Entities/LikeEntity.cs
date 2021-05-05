using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.DataAccess.Entities
{
    /// <summary>
    /// This class represents the like entity DTO that transfers data from the service to the DAO.
    /// </summary>
    public class LikeEntity
    {
        // Represents the unique identifier of a post.
        public string PostId { get; set; }

        // Represents the unique identifier of a user.
        public string UserId { get; set; }
    }
}
