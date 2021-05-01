using System;
using System.Collections.Generic;
using AutoBuildApp.DataAccess;
using AutoBuildApp.DataAccess.Abstractions;
using AutoBuildApp.Models;
using AutoBuildApp.Models.Interfaces;

/**
* The ShelfServices will provide the shelf specific operatons.
* Catches errors, prepares commmon response object.
* @Author Nick Marshall-Eminger
*/
namespace AutoBuildApp.Services
{
    public class ShelfService
    {
        private ShelfDAO _dao;

        public ShelfService(ShelfDAO shelfDAO)
        {
            _dao = shelfDAO;
        }

        public IMessageResponse CreateShelf(string shelfName, string user)
        {
            IMessageResponse output = new StringBoolResponse();

            try
            {
                output.SuccessBool = _dao.InsertShelf(shelfName, user);
                output.MessageString = ResponseStringGlobals.SUCCESSFUL_CREATION;
            }
            catch (TimeoutException)
            {
                output.SuccessBool = false;
                output.MessageString = ResponseStringGlobals.CALL_TIMEOUT;
            }

            return output;
        }

        public IMessageResponse DeleteShelf(string shelfID)
        {
            IMessageResponse output = new StringBoolResponse();
            bool success = _dao.DeleteShelf(shelfID);

            return output;
        }

        public IMessageResponse ChangeShelfName(string oldName, string newName, string user)
        {
            IMessageResponse output = new StringBoolResponse();
            return output;
        }

        public IMessageResponse AddToShelf(IComponent item, string shelfName, string user)
        {
            IMessageResponse output = new StringBoolResponse();
            return output;
        }

        public IMessageResponse RemoveFromShelf(int index, string shelfName)
        {
            IMessageResponse output = new StringBoolResponse();
            return output;
        }

        public IMessageResponse ModifyShelf(int indexStart, int indexEnd, string user)
        {
            IMessageResponse output = new StringBoolResponse();
            return output;
        }

        public IMessageResponse ChangeQuantity(int count, string itemID, string shelfName)
        {
            IMessageResponse output = new StringBoolResponse();
            return output;
        }

        public List<IComponent> GetShelf(string shelfName)
        {
            List<IComponent> outputList = new List<IComponent>();

            return outputList;
        }

        public IComponent GetComponent(int index, string shelfName)
        {
            IComponent output = null;

            return output;
        }
    }
}