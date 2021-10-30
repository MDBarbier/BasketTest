using BasketTestLib.Exceptions;
using BasketTestLib.Interfaces;
using BasketTestLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BasketTestLib.Services
{
    /// <summary>
    /// Singleton class (thread safe) which exposes basket functionality. Allows the creation of a new basket, and retrieval of existing baskets by their GUID.
    /// </summary>
    public class BasketService
    {
        #region Properties

        public Dictionary<Guid, IBasket> Baskets { get; set; }
        public Guid Guid { get; set; }

        #endregion

        #region Local Fields

        ICodeCheckService _codeCheckService;
        private static BasketService _singletonInstance;
        private static readonly object _lock = new();    
        
        #endregion

        #region Constructors

        private BasketService(ICodeCheckService codeCheckService)
        {            
            _codeCheckService = codeCheckService;
            Baskets = new Dictionary<Guid, IBasket>();
            Guid = Guid.NewGuid();
        }

        #endregion

        #region Instance methods

        public static BasketService GetInstance(ICodeCheckService codeCheckService)
        {
            if (_singletonInstance == null)
            {
                lock (_lock)
                {
                    if (_singletonInstance == null)
                    {
                        _singletonInstance = new BasketService(codeCheckService);                        
                    }
                }
            }

            return _singletonInstance;
        }

        public IBasket GetBasket(Guid? guid)
        {
            if (guid == null)
            {
                IBasket temp;
                lock (_lock)
                {
                    temp = new Basket(_codeCheckService);
                    var newGuid = Guid.NewGuid();
                    temp.BasketGuid = newGuid;
                    Baskets.Add(newGuid, temp); 
                }
                return temp;
            }

            if (guid != null && Baskets.ContainsKey((Guid)guid))
            {
                return Baskets.Where(item => item.Key == guid).Select(item => item.Value).FirstOrDefault();
            }
            else
            {
                throw new BasketNotFoundException("Basket with supplied guid not found");
            }
        }

        #endregion
    }
}
