using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Models.Reflections
{
    public class DAOAttribute : Attribute
    {
        public DAOAttribute():base()
        {
        }

        private string _DatabaseColumn;

        private Type _ValueType;

        private bool _PrimaryKey;

        public string DatabaseColumn
        {
            get
            {
                return this._DatabaseColumn;
            }
            set
            {
                _DatabaseColumn = value;
            }
        }

        public Type ValueType
        {
            get
            {
                return this._ValueType;
            }
            set
            {
                _ValueType = value;
            }
        }

        public bool PrimaryKey
        {
            get
            {
                return this._PrimaryKey;
            }
            set
            {
                _PrimaryKey = value;
            }
        }

        public DAOAttribute(string databaseColumn, Type valueType, bool primaryKey) : base()
        {
            _DatabaseColumn = databaseColumn;
            _ValueType = valueType;
            _PrimaryKey = primaryKey;

        }
    }
}
