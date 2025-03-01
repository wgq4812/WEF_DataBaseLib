﻿/*
* 描述： 详细描述类能干什么
* 创建人：wenli
* 创建时间：2017/3/2 14:26:21
*/
/*
*修改人：wenli
*修改时间：2017/3/2 14:26:21
*修改内容：xxxxxxx
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using WEF.Common;
using WEF.Expressions;
using WEF.Models;
using WEF.Test.Models;

namespace WEF.Test
{
    class Program
    {
        static void Main(string[] args)
        {

            //MySqlBitTypeTest.Test1();
            MySqlBitTypeTest.Test3();

            //BatchTest.Test();

            //MutiTablesTest.Test();

            //BatchTest2.Test();

            //new DBTicketOrderRepository().Search().Where(b => b.C_id == "123sdf4asdfsadfgrewfdg5498432165").OrderBy(b=>b.C_price).ToFirst();

            //Test4();

            //Test3();

            Console.ReadLine();

            var c_id = Guid.NewGuid().ToString("N");

            var dbtickerOrder = new DBTicketOrder()
            {
                C_id = c_id,
                C_supplier = 1,
                C_appId = "s",
                C_channelId = "22",
                C_sku = "sss",
                C_outTradeNo = "afefa",
                C_count = 1,
                C_phone = "1111",
                C_nonce = "asfew",
                C_timestamp = "asdfeaf",
                C_sign = "afefqwaf",
                C_amount = 19.80M,
                C_discountrate = 90F,
                C_activityName = "asdfasd",
                C_productName = "asdfed",
                C_resv1 = "hello",
                C_created = DateTime.Now
            };
            



            var c_price = (decimal)(((float)dbtickerOrder.C_amount) * dbtickerOrder.C_discountrate / 100F);

            dbtickerOrder.C_price = c_price;

            if (new DBTicketOrderRepository().Insert(dbtickerOrder) > 0)
            {
                Console.WriteLine($"DBTicketOrder数据插入成功,{dbtickerOrder.C_price}");

                var newdbtickerOrder = new DBTicketOrderRepository().GetDBTicketOrder(dbtickerOrder.C_id);

                Console.WriteLine($"DBTicketOrder数据查询成功,{newdbtickerOrder.C_price}");

                newdbtickerOrder.C_updated = DateTime.Now;

                newdbtickerOrder.C_resv1 = null;

                if (new DBTicketOrderRepository().Update(newdbtickerOrder) > 0)
                {
                    var newdbtickerOrder2 = new DBTicketOrderRepository().GetDBTicketOrder(c_id);

                    Console.WriteLine($"DBTicketOrder数据查询成功,{newdbtickerOrder2.C_price}");
                }
            }

            new DBTicketOrderRepository().Delete(c_id);

            Console.ReadLine();


            var db = new DBContext();

            var dt = db.FromSql("select * from tb_task").ToDataTable();

            var dttasks = dt.DataTableToEntityList<DBTask>();

            var taskModel = new TaskModel()
            {
                Crc32 = 123,
                TaskID = Guid.NewGuid().ToString("N"),
                BeginTime = DateTime.Now,
                BusinID = "asdfead24545",
                CompanyId = "54",
                CreateTime = DateTime.Now,
                DayLimit = 10,
                DayTimes = 100,
                Description = "𠂆𠂆𠂆𠂆𠂆𠂆𠂆𠂆",
                EndTime = DateTime.Now,
                FlowType = "5687653",
                IsDel = false,
                IsEnabled = true,
                Name = "afeadfad545646546",
                Operator = "5435635321",
                PlatformID = "8423416534635",
                Point = 2,
                TaskMaxPoint = 100,
                TaskType = 3
            };



            var tn = taskModel.ConvertTo<DBTask>();

            var insertResult = new DBTaskRepository().Insert(tn);

            var taa = taskModel.ConvertTo<TestA>();

            var aa = new TestA()
            {
                aa = DateTime.Now,
                Age = 10,
                //Created = "2019-04-22 22:53",
                id = "100001",
                Num = 10.235M,
                Num1 = 100
            };

            var bb = aa.ConvertTo<TestB>();

            var cc = bb.ConvertTo<TestA>();

            Console.WriteLine("WEF使用实例");

            Console.WriteLine("-----------------------------");



            var tasks = new DBTaskRepository().Search().ToList();

            DBUserTaskRepository userTaskRepository = null;

            var ts1 = Task.Factory.StartNew(() =>
            {
                userTaskRepository = new DBUserTaskRepository();
            });

            Task.WaitAll(ts1);

            var useTasks1 = userTaskRepository.Search().Where(b => b.Isenabled).ToList();

            var useTasks2 = userTaskRepository.Search().Where(b => b.Userid == "6312124585351742" && b.Isenabled).ToList();


            for (int i = 0; i < 10000; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        userTaskRepository.Search().First();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                });
            }


            Console.ReadLine();

            new DBTaskRepository().Delete("");

            var giftopt = new DBGiftRepository();

            var giftwhere = giftopt.Search().Where(b => b.Giftid.Contains("1"));

            giftwhere = giftwhere.Where(b => !b.Isdel && b.Isenabled); //不能连接


            giftwhere = giftwhere.OrderBy(b => b.Createtime).OrderByDescending(b => b.Giftid);//不能连接

            var glist = giftwhere.ToList();

            var glist2 = giftopt.Search().Where(b => b.Giftid.Like("1") && b.Isdel.IsNotNull()).ToList();

            var giftids = giftopt.Search().Page(1, 10).ToList().Select(b => b.Giftid).ToList();

            var sum = giftopt.Search().Select(b => b.Supporttype.Sum()).ToFirstDefault().Supporttype;

            var avg = giftopt.Search().Select(b => b.Supporttype.Avg()).ToFirstDefault().Supporttype;

            var glist22 = giftopt.Search().Where(b => b.Giftid.In(giftids)).ToList();

            var gids = new List<string>();

            gids.Add("120100010219094341");
            gids.Add("030000050310180911");
            gids.Add("201706260157165");
            gids.Add("201706300150728");

            var glist3 = giftopt.Search().Where(b => b.Giftid.In(gids)).ToList();


            #region where

            //不支持
            DBUserPointRepository tb_UserpointRepository = new DBUserPointRepository();

            var upWhere1 = tb_UserpointRepository.Search();

            var w1 = upWhere1.Where(b => b.Uid == "sss");

            var w2 = w1.Where(b => b.Points > 0);

            var up1 = upWhere1.First();

            //支持
            Expression<Func<DBUserPoint, bool>> eWhere1 = null;

            if (true)
            {
                eWhere1 = (b => b.Uid.IsNotNull());
            }

            Expression<Func<DBUserPoint, bool>> eWhere2 = (b => b.Points > 0);

            if (true)
            {
                eWhere2 = (b => b.Points > 0);
            }

            Expression<Func<DBUserPoint, bool>> eWhere3 = null;

            if (false)
            {
                eWhere3 = (b => b.Uid.Contains("1"));
            }


            var upWhere2 = tb_UserpointRepository.Search().Where(eWhere1, eWhere2, eWhere3).ToList();


            //tb_UserpointRepository.Search().OrderBy()


            //Where条件拼接一：

            var dbTaskRepository = new DBTaskRepository();

            var where1 = new Where<DBTask>();
            where1.And(d => d.Operator != "");
            where1.And(d => d.Totallimit >= 0);

            var list1 = dbTaskRepository.Search()
                            .Where(where1)
                            .Page(1, 2)
                            .ToList();

            var list2 = dbTaskRepository.Search()
                            .Where(where1)
                            .Page(2, 2)
                            .ToList();

            //多表条件拼接

            //var where2 = new Where<table>();
            //where2.And(a => a.id == 1);
            //where2.And<table2>((a, b) => b.id == 2);
            //where2.And<table3>((a, c) => c.id == 3);

            //var list2 = new DBContext().Search<table>()
            //                .InnerJoin<table2>((a, b) => a.id == b.aid)
            //                .InnerJoin<table3>((a, c) => a.id == c.aid)
            //                .Where(where1)
            //                .ToList();

            //上面的where还可以这样写：
            //var where3 = new Where<table>();
            //where3.And<table2, table3>((a, b, c) => a.id == 1 && b.id == 2 && c.id == 3);

            #endregion


            var plist = tb_UserpointRepository.GetList(1, 100);

            #region mysql

            DBTaskRepository repository = new DBTaskRepository();

            var task = repository.GetList(1, 10);

            //var taskModel = task.ConvertTo<TaskModel>();

            #endregion


            #region 无实体sql操作，自定义参数            

            var dt1 = new DBContext().FromSql("select * from tb_task where taskid=@taskID").AddInParameter("@taskID", System.Data.DbType.String, 200, "10B676E5BC852464DE0533C5610ACC53").ToFirst<DBTask>();

            var count1 = new DBContext().Search<DBTask>().Where(b => b.Crc32.Avg() > 1).Where(" 1=1 ").Count();

            var dt2 = new DBContext().FromSql("select * from tb_task where taskid=@taskID").AddInParameter("@taskID", "10B676E5BC852464DE0533C5610ACC53").ToFirst<DBTask>();

            var count2 = new DBContext().Search<DBTask>().Where(b => b.Crc32.Avg() > 1).Where(" 1=1 ").Count();

            var dt3 = new DBContext().FromSql("select * from tb_task where taskid=@taskID").AddInParameterWithModel(new { taskID = "10B676E5BC852464DE0533C5610ACC53" }).ToFirst<DBTask>();

            var count3 = new DBContext().Search<DBTask>().Where(b => b.Crc32.Avg() > 1).Where(" 1=1 ").Count();

            #endregion


            string result = string.Empty;

            var entity = new Models.ArticleKind();

            var entityRepository = new Models.ArticleKindRepository();

            var pagedList = entityRepository.Search(entity).GetPagedList(1, 100, "ID", true);

            do
            {
                Test2();

                Console.WriteLine("输入R继续,其他键退出");
                result = Console.ReadLine();
            }
            while (result.ToUpper() == "R");
        }

        static void Test2()
        {

            UserRepository ur = new UserRepository();

            var e = ur.Search().Where(b => b.NickName == "adsfasdfasdf").First();

            var ut = new User()
            {
                ID = Guid.NewGuid(),
                ImUserID = "",
                NickName = "张三三"
            };

            var r = ur.Insert(ut);

            var count = ur.Search().Count();

            ut.NickName = "李四四";

            //ut.ConvertTo

            r = ur.Update(ut);

            #region search 

            var search = ur.Search().Where(b => b.NickName.Like("张*"));

            search = search.Where(b => !string.IsNullOrEmpty(b.ImUserID));

            var rlts = search.Page(1, 20).ToList();

            #endregion


            using (var batch = ur.DBContext.CreateBatch<User>())
            {
                batch.Insert(ut);
            }

            var nut = ut.ConvertTo<SUser>();

            var nut1 = ut.ConvertTo<SUser>();

            var nnut = nut.ConvertTo<User>();

            var ults = ur.GetList(1, 1000);

            r = ur.Delete(ut);



            #region tran

            var tran1 = ur.DBContext.BeginTransaction();

            try
            {
                tran1.Insert(ut);

                var tb1 = new DBTaskRepository().GetList(1, 10);

                tran1.Update(tb1);

                tran1.Commit();
            }
            catch
            {
                tran1.Rollback();
            }
            finally
            {
                ur.DBContext.CloseTransaction(tran1);
            }

            //or
            using (var tran2 = ur.DBContext.BeginTransaction())
            {
                tran2.Insert(ut);

                new DBTaskRepository().DBContext.Delete(tran2, new DBTask() { Taskid = "123" });

                tran2.Delete(ut);

                tran2.Commit();
            }

            #endregion


            var dlts = ur.GetList(1, 10000);
            ur.Deletes(dlts);

        }


        public class TestA
        {
            public DateTime aa { get; set; }

            public string id
            {
                get; set;
            }

            public int? Age
            {
                get; set;
            }

            public string Created
            {
                get; set;
            }

            public decimal? Num
            {
                get; set;
            }

            public int Num1
            {
                get; set;
            }
        }

        public class TestB
        {
            public int ID
            {
                get; set;
            }

            public string age
            {
                get; set;
            }

            public DateTime? created
            {
                get; set;
            }

            public int bb { get; set; }

            public int? Num
            {
                get; set;
            }

            public decimal? Num1
            {
                get; set;
            }
        }


        static void Test3()
        {
            var list1 = new DBTaskRepository().Search().GetPagedList(1, 20, "begintime", true).Select(b => b.Begintime).ToList();

            var list2 = new DBTaskRepository().Search().GetPagedList(1, 20, "begintime", false).Select(b => b.Begintime).ToList();

            if (list1.First() == list2.First())
            {

            }
        }

        static void Test4()
        {
            UsersRepository usersRepository = new UsersRepository(DatabaseType.PostgreSQL, "PORT=5432;DATABASE=test;HOST=127.0.0.1;PASSWORD=yswenli;USER ID=postgres;");

            var id = Guid.NewGuid().GetHashCode();

            var r = usersRepository.Insert(new Users()
            {
                Id = id,
                Name = "chewang",
                Pwd = "12321",
                Created = DateTime.Now
            });

            var e = usersRepository.GetUsers(id);


            e.Name = "Chewang";

            usersRepository.Update(e);


            var l = usersRepository.Search().Where(b => b.Id > 0).OrderBy(b => b.Created).Page(1, 10).ToList();

            Console.ReadLine();

        }


    }
}
