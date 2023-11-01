using Microsoft.AspNetCore.Mvc;
using work_sessions.Models;

namespace work_sessions.Controllers
{
    
    public class ShoppingController : Controller
    {
        private readonly ShopDbContext db;
        public ShoppingController(ShopDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            ViewBag.msg = TempData["msg"];
            
            return View(db.Products.ToList());
        }
        public IActionResult AddToCart(int pId,double qty)
        {
            bool itemFound=false;
            string msg = "";
            if(pId > 0 && qty>0)
            {
                var prod=db.Products.FirstOrDefault(p => p.Id == pId);
                if(prod!=null)
                {
                    List<Product>items=new List<Product>();
                    var xItems = HttpContext.Session.GetObject<List<Product>>("cart");
                    if(xItems!=null)
                    {
                        foreach(var item in xItems)
                        {
                            if(pId == item.Id)
                            {
                                item.Quantity += qty;
                                itemFound = true;
                                msg = "Item already added , new quantity is added !!!";
                            }
                            items.Add(item);
                        }
                        if( !itemFound )
                        {
                            prod.Quantity = qty;
                            items.Add(prod);
                            msg="New item id added with existing item !!";

                        }
                        HttpContext.Session.SetObject<List<Product>>("cart", items);
                    }
                    else
                    {
                        prod.Quantity= qty;
                        items.Add(prod);
                        HttpContext.Session.SetObject<List<Product>>("cart", items);
                        msg = "New item is added to empty card !!";
                    }
                }
                else
                {
                    msg = "Item not found !!";
                }
            }
            else
            {
                msg = "Item id could not be zero!!!!";
            }
            TempData["msg"] = msg;
            return RedirectToAction("Index");
            
        }
        public IActionResult ShowCart()
        {
            List<Product> items = HttpContext.Session.GetObject<List<Product>>("cart");
            if(items!=null &&items.Count !=0)
            {
                return View(items.ToList());
            }
            else
            {
                items=new List<Product>();
                return View();
            }
        }
        public IActionResult RemoveFormCart(int ? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            List<Product> productList = HttpContext.Session.GetObject<List<Product>>("cart");
            var removeItems=productList.FirstOrDefault(x => x.Id==id);
            productList.Remove(removeItems);
            HttpContext.Session.SetObject("cart", productList);
            return RedirectToAction("ShowCart");
            
        }
        public IActionResult CheckOut()
        {
            var us = HttpContext.Session.GetString("un");
            var id = HttpContext.Session.GetString("id");
            if(us != null)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(nameof(Login));
            }
        }
        public IActionResult ConfirmOrder()
        {
            return View();
        }
        public IActionResult Login(AppUser appUser)
        {
            var userName=db.AppUsers.FirstOrDefault(x=>x.UserName==appUser.UserName);
            var password=db.AppUsers.FirstOrDefault(x=>x.Password==appUser.Password);
            if (userName == null && password == null)
            {
                HttpContext.Session.SetString("un", appUser.UserName);
                HttpContext.Session.SetString("id", appUser.Password);
                return RedirectToAction(nameof(ConfirmOrder));
            }
            else
            {
                TempData["wrongInfo"] = "Wrong Information !!";
                return View(appUser);
            }
            
        }
        public IActionResult signUp()
        {
            return View(); 
        }
        [HttpPost]
     
        public async Task <IActionResult> SignUp([Bind("UserName","Password")]AppUser appUser)
        {
            string msg = "";
            if (ModelState.IsValid)
            {
                appUser.Role = 1;
                appUser.IsActive = 1;
                var checkUserName=db.AppUsers.FirstOrDefault(x=>x.UserName.ToLower()==appUser.UserName.ToLower());
                if(checkUserName != null)
                {
                    TempData["signUp"] = "Successfully Create a Account !!";
                }   
                db.AppUsers.Add(appUser);
                await db.SaveChangesAsync();
                HttpContext.Session.SetString("un",appUser.UserName);
                HttpContext.Session.SetString("id",appUser.UserName);
                appUser.UserName = null;
                appUser.Password = null;
                TempData["signUp"] = "Successfully Create an account!!";
                return RedirectToAction(nameof(Login));
            }
            else
            {
                msg = " Username already exist !!";
                return View(appUser);
            }
            return View();
        }
    }
}
