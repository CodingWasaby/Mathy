using Mathy.Shared.Entity;
using Mathy.Shared.Page;
using Mathy.Shared.Search;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Mathy.Repository.Repo
{
    public class UserRepo : BaseRepo
    {
        public UserRepo(DbBase dbBase) : base(dbBase)
        {

        }

        public PageList<User> GetUSers(USerSearch search)
        {
            search.Page.OrderField = "p.CreateTime";
            string sql = @" SELECT  *
                            FROM    dbo.UserDB (NOLOCK)
                            WHERE   DeleteFlag = 0 ";
            if (!string.IsNullOrEmpty(search.UserName))
            {
                sql += " AND UserName Like '%" + search.UserName + "%'";
            }
            if (!string.IsNullOrEmpty(search.Company))
            {
                sql += " AND Company Like '%" + search.Company + "%'";
            }
            if (search.BeginDate.HasValue)
            {
                sql += " AND CreateTime > @BeginDate ";
            }
            if (search.EndDate.HasValue)
            {
                sql += " AND CreateTime < @EndDate ";
            }
            return QueryPage<User>(sql, search, search.Page);
        }

        public User GetUser(string email)
        {
            string sql = @" SELECT  *
                            FROM    dbo.UserDB (NOLOCK)
                            WHERE   DeleteFlag = 0
                                    AND Email = @Email ";
            return QueryFirst<User>(sql, new { Email = email });
        }

        public User GetUser(string email, string passWord)
        {
            string sql = @" SELECT  *
                            FROM    dbo.UserDB (NOLOCK)
                            WHERE   DeleteFlag = 0
                                    AND Email = @Email AND PassWord=@PassWord";
            return QueryFirst<User>(sql, new { Email = email, PassWord = passWord });
        }

        public int AddUser(User user)
        {
            return ExcuteScalar(@"INSERT  INTO dbo.UserDB
                                        ( Email ,
                                          Password ,
                                          Name ,
                                          CreateTime ,
                                          Company ,
                                          CellPhone ,
                                          TelPhone ,
                                          EnableDate ,
                                          DeleteFlag
                                        )
                                VALUES  ( @Email ,
                                          @Password ,
                                          @Name ,
                                          @CreateTime ,
                                          @Company ,
                                          @CellPhone ,
                                          @TelPhone ,
                                          @EnableDate ,
                                          0
                                        ) SELECT @@IDENTITY ", user);
        }

        public bool DeleteUser(string email, SqlTransaction transaction = null)
        {
            string sql = @" UPDATE dbo.UserDB SET DeleteFlag =1 WHERE Email = @Email";
            return Excute(sql, new { Email = email }, transaction);
        }

        public bool UpdateUserInfo(User userEntity, SqlTransaction transaction = null)
        {
            string sql = @"UPDATE  dbo.UserDB
                            SET     Name = @Name ,
                                    Company = @Company ,
                                    CellPhone = @CellPhone ,
                                    TelPhone = @TelPhone
                            WHERE   Email = @Email AND DeleteFlag = 0 ";
            return Excute(sql, userEntity, transaction);
        }

        public bool UpdateUserEnableDate(User userEntity, SqlTransaction transaction = null)
        {
            string sql = @" UPDATE  dbo.UserDB
                            SET    EnableDate = @EnableDate
                            WHERE   Email = @Email  AND DeleteFlag =0 ";
            return Excute(sql, userEntity, transaction);
        }

        public bool UpdateUserPass(User userEntity, SqlTransaction transaction = null)
        {
            string sql = @" UPDATE  dbo.UserDB
                            SET    Password = @Password
                            WHERE   Email = @Email  AND DeleteFlag =0 ";
            return Excute(sql, userEntity, transaction);
        }
    }
}
