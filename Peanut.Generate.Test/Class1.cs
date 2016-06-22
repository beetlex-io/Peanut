using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Peanut.Mappings;

namespace Peanut.Generate.Test
{
    ///<summary>
    ///Type:string
    ///</summary>
    public partial class DbBlock : Peanut.Mappings.DataObject
    {
        private string mID;
        public static Peanut.FieldInfo<string> iD = new Peanut.FieldInfo<string>("Block", "sdfsd");
        private string mTitle;
        public static Peanut.FieldInfo<string> title = new Peanut.FieldInfo<string>("Block", "Title");
        private string mData;
        public static Peanut.FieldInfo<string> data = new Peanut.FieldInfo<string>("Block", "Data");
        private string mCategoryID;
        public static Peanut.FieldInfo<string> categoryID = new Peanut.FieldInfo<string>("Block", "CategoryID");
        ///<summary>
        ///Type:string
        ///</summary>
        [ID("sdfsd")]
        [UID]
        public virtual string ID
        {
            get
            {
                return mID;

            }
            set
            {
                mID = value;
                EntityState.FieldChange("ID");

            }

        }
        ///<summary>
        ///Type:string
        ///</summary>
        [Column()]
        [DefaultInt(1)]
        public virtual string Title
        {
            get
            {
                return mTitle;

            }
            set
            {
                mTitle = value;
                EntityState.FieldChange("Title");

            }

        }
        ///<summary>
        ///Type:string
        ///</summary>
        [Column()]
        public virtual string Data
        {
            get
            {
                return mData;

            }
            set
            {
                mData = value;
                EntityState.FieldChange("Data");

            }

        }
        ///<summary>
        ///Type:string
        ///</summary>
        [Column()]
        public virtual string CategoryID
        {
            get
            {
                return mCategoryID;

            }
            set
            {
                mCategoryID = value;
                EntityState.FieldChange("CategoryID");

            }

        }

    }
    ///<summary>
    ///Type:string
    ///</summary>
    public partial class DbBlockCategory : Peanut.Mappings.DataObject
    {
        private string mID;
        public static Peanut.FieldInfo<string> iD = new Peanut.FieldInfo<string>("BlockCategory", "ID");
        private string mName;
        public static Peanut.FieldInfo<string> name = new Peanut.FieldInfo<string>("BlockCategory", "Name");
        ///<summary>
        ///Type:string
        ///</summary>
        [ID()]
        [UID]
        public virtual string ID
        {
            get
            {
                return mID;

            }
            set
            {
                mID = value;
                EntityState.FieldChange("ID");

            }

        }
        ///<summary>
        ///Type:string
        ///</summary>
        [Column()]
        public virtual string Name
        {
            get
            {
                return mName;

            }
            set
            {
                mName = value;
                EntityState.FieldChange("Name");

            }

        }

    }
    ///<summary>
    ///Type:string
    ///</summary>
    public partial class File : Peanut.Mappings.DataObject
    {
        private string mID;
        public static Peanut.FieldInfo<string> iD = new Peanut.FieldInfo<string>("File", "ID");
        private string mName;
        public static Peanut.FieldInfo<string> name = new Peanut.FieldInfo<string>("File", "Name");
        ///<summary>
        ///Type:string
        ///</summary>
        [ID()]
        [UID]
        public virtual string ID
        {
            get
            {
                return mID;

            }
            set
            {
                mID = value;
                EntityState.FieldChange("ID");

            }

        }
        ///<summary>
        ///Type:string
        ///</summary>
        [Column()]
        public virtual string Name
        {
            get
            {
                return mName;

            }
            set
            {
                mName = value;
                EntityState.FieldChange("Name");

            }

        }

    }
    ///<summary>
    ///Type:int
    ///</summary>
    public partial class PostCategory : Peanut.Mappings.DataObject
    {
        private string mID;
        public static Peanut.FieldInfo<string> iD = new Peanut.FieldInfo<string>("PostCategory", "ID");
        private string mName;
        public static Peanut.FieldInfo<string> name = new Peanut.FieldInfo<string>("PostCategory", "Name");
        private int mPosts;
        public static Peanut.FieldInfo<int> posts = new Peanut.FieldInfo<int>("PostCategory", "Posts");
        ///<summary>
        ///Type:string
        ///</summary>
        [ID()]
        [UID]
        public virtual string ID
        {
            get
            {
                return mID;

            }
            set
            {
                mID = value;
                EntityState.FieldChange("ID");

            }

        }
        ///<summary>
        ///Type:string
        ///</summary>
        [Column()]
        public virtual string Name
        {
            get
            {
                return mName;

            }
            set
            {
                mName = value;
                EntityState.FieldChange("Name");

            }

        }
        ///<summary>
        ///Type:int
        ///</summary>
        [Column()]
        public virtual int Posts
        {
            get
            {
                return mPosts;

            }
            set
            {
                mPosts = value;
                EntityState.FieldChange("Posts");

            }

        }

    }
    ///<summary>
    ///Type:string
    ///</summary>
    public partial class Post : Peanut.Mappings.DataObject
    {
        private string mID;
        public static Peanut.FieldInfo<string> iD = new Peanut.FieldInfo<string>("Post", "ID");
        private string mTitle;
        public static Peanut.FieldInfo<string> title = new Peanut.FieldInfo<string>("Post", "Title");
        private string mSummary;
        public static Peanut.FieldInfo<string> summary = new Peanut.FieldInfo<string>("Post", "Summary");
        private DateTime mCreateTime;
        public static Peanut.FieldInfo<DateTime> createTime = new Peanut.FieldInfo<DateTime>("Post", "CreateTime");
        private string mCategories;
        public static Peanut.FieldInfo<string> categories = new Peanut.FieldInfo<string>("Post", "Categories");
        private string mData;
        public static Peanut.FieldInfo<string> data = new Peanut.FieldInfo<string>("Post", "Data");
        ///<summary>
        ///Type:string
        ///</summary>
        [ID()]
        [UID]
        public virtual string ID
        {
            get
            {
                return mID;

            }
            set
            {
                mID = value;
                EntityState.FieldChange("ID");

            }

        }
        ///<summary>
        ///Type:string
        ///</summary>
        [Column()]
        public virtual string Title
        {
            get
            {
                return mTitle;

            }
            set
            {
                mTitle = value;
                EntityState.FieldChange("Title");

            }

        }
        ///<summary>
        ///Type:string
        ///</summary>
        [Column()]
        public virtual string Summary
        {
            get
            {
                return mSummary;

            }
            set
            {
                mSummary = value;
                EntityState.FieldChange("Summary");

            }

        }
        ///<summary>
        ///Type:DateTime
        ///</summary>
        [Column()]
        public virtual DateTime CreateTime
        {
            get
            {
                return mCreateTime;

            }
            set
            {
                mCreateTime = value;
                EntityState.FieldChange("CreateTime");

            }

        }
        ///<summary>
        ///Type:string
        ///</summary>
        [Column()]
        public virtual string Categories
        {
            get
            {
                return mCategories;

            }
            set
            {
                mCategories = value;
                EntityState.FieldChange("Categories");

            }

        }
        ///<summary>
        ///Type:string
        ///</summary>
        [Column()]
        public virtual string Data
        {
            get
            {
                return mData;

            }
            set
            {
                mData = value;
                EntityState.FieldChange("Data");

            }

        }

    }
    ///<summary>
    ///Type:DateTime
    ///</summary>
    public partial class CategoryToPost : Peanut.Mappings.DataObject
    {
        private string mPostID;
        public static Peanut.FieldInfo<string> postID = new Peanut.FieldInfo<string>("CategoryToPost", "PostID");
        private string mCategoryID;
        public static Peanut.FieldInfo<string> categoryID = new Peanut.FieldInfo<string>("CategoryToPost", "CategoryID");
        private DateTime mCreateTime;
        public static Peanut.FieldInfo<DateTime> createTime = new Peanut.FieldInfo<DateTime>("CategoryToPost", "CreateTime");
        ///<summary>
        ///Type:string
        ///</summary>
        [Column()]
        public virtual string PostID
        {
            get
            {
                return mPostID;

            }
            set
            {
                mPostID = value;
                EntityState.FieldChange("PostID");

            }

        }
        ///<summary>
        ///Type:string
        ///</summary>
        [Column()]
        public virtual string CategoryID
        {
            get
            {
                return mCategoryID;

            }
            set
            {
                mCategoryID = value;
                EntityState.FieldChange("CategoryID");

            }

        }
        ///<summary>
        ///Type:DateTime
        ///</summary>
        [Column()]
        [NowDate]
        public virtual DateTime CreateTime
        {
            get
            {
                return mCreateTime;

            }
            set
            {
                mCreateTime = value;
                EntityState.FieldChange("CreateTime");

            }

        }

    }
    ///<summary>
    ///Type:string
    ///</summary>
    public partial class PostJoinCategory : Peanut.Mappings.DataObject
    {
        private string mID;
        public static Peanut.FieldInfo<string> iD = new Peanut.FieldInfo<string>("Post inner join CategoryToPost on Post.id = CategoryToPost.PostID", "ID");
        private string mTitle;
        public static Peanut.FieldInfo<string> title = new Peanut.FieldInfo<string>("Post inner join CategoryToPost on Post.id = CategoryToPost.PostID", "Title");
        private string mSummary;
        public static Peanut.FieldInfo<string> summary = new Peanut.FieldInfo<string>("Post inner join CategoryToPost on Post.id = CategoryToPost.PostID", "Summary");
        private DateTime mCreateTime;
        public static Peanut.FieldInfo<DateTime> createTime = new Peanut.FieldInfo<DateTime>("Post inner join CategoryToPost on Post.id = CategoryToPost.PostID", "CreateTime");
        private string mCategories;
        public static Peanut.FieldInfo<string> categories = new Peanut.FieldInfo<string>("Post inner join CategoryToPost on Post.id = CategoryToPost.PostID", "Categories");
        private string mData;
        public static Peanut.FieldInfo<string> data = new Peanut.FieldInfo<string>("Post inner join CategoryToPost on Post.id = CategoryToPost.PostID", "Data");
        ///<summary>
        ///Type:string
        ///</summary>
        [Column()]
        public virtual string ID
        {
            get
            {
                return mID;

            }
            set
            {
                mID = value;
                EntityState.FieldChange("ID");

            }

        }
        ///<summary>
        ///Type:string
        ///</summary>
        [Column()]
        public virtual string Title
        {
            get
            {
                return mTitle;

            }
            set
            {
                mTitle = value;
                EntityState.FieldChange("Title");

            }

        }
        ///<summary>
        ///Type:string
        ///</summary>
        [Column()]
        public virtual string Summary
        {
            get
            {
                return mSummary;

            }
            set
            {
                mSummary = value;
                EntityState.FieldChange("Summary");

            }

        }
        ///<summary>
        ///Type:DateTime
        ///</summary>
        [Column()]
        public virtual DateTime CreateTime
        {
            get
            {
                return mCreateTime;

            }
            set
            {
                mCreateTime = value;
                EntityState.FieldChange("CreateTime");

            }

        }
        ///<summary>
        ///Type:string
        ///</summary>
        [Column()]
        public virtual string Categories
        {
            get
            {
                return mCategories;

            }
            set
            {
                mCategories = value;
                EntityState.FieldChange("Categories");

            }

        }
        ///<summary>
        ///Type:string
        ///</summary>
        [Column()]
        public virtual string Data
        {
            get
            {
                return mData;

            }
            set
            {
                mData = value;
                EntityState.FieldChange("Data");

            }

        }

    }

}
