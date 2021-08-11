
namespace EazyGF
{

    class ShopLogic
    {
        static ShopLogic shopLogic = null;

        public static ShopLogic Intance
        {
            get
            {
                if (shopLogic == null)
                {
                    return new ShopLogic();
                }

                return shopLogic;
            } 
        }

    }
}
