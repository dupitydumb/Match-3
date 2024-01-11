//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Utilities.Service
//{
//    [RequireComponent(typeof(GamePayment))]
//    public class PaymentHelper : MonoBehaviour
//    {
//        #region Members

//        private static PaymentHelper mInstance;
//        public static PaymentHelper Instance
//        {
//            get
//            {
//                if (mInstance == null)
//                    mInstance = FindObjectOfType<PaymentHelper>();
//                return mInstance;
//            }
//        }

//        #endregion

//        //=============================================

//        #region MonoBehaviour

//        private void Awake()
//        {
//            if (mInstance == null)
//                mInstance = this;
//            else if (mInstance != this)
//                Destroy(gameObject);
//        }

//        #endregion

//        //=============================================

//        #region Public

//        public void InitProducts(List<string> skus, Action<bool> pOnFinished)
//        {
//            GamePayment.Instance.Init(skus, true, pOnFinished);
//        }

//        public void Purchase(string sku, Action<bool> pAction)
//        {
//            GamePayment.Instance.Purchase(sku, pAction);
//        }

//        public DateTime? SubscriptionExpireDate(string pPackageId)
//        {
//#if UNITY_IAP
//            var subscriptionInfo = GamePayment.Instance.GetSubscriptionInfo(pPackageId);
//            if (subscriptionInfo == null)
//                return null;

//            return subscriptionInfo.getExpireDate();
//#else
//            return null;
//#endif
//        }

//        public decimal GetLocalizedPrice(string pPackageId)
//        {
//            return GamePayment.Instance.GetLocalizedPrice(pPackageId);
//        }

//        public string GetLocalizedPriceString(string pPackageId)
//        {
//            return GamePayment.Instance.GetLocalizedPriceString(pPackageId);
//        }

//        #endregion
//    }
//}