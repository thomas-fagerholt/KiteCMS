using KiteCMS.Admin.core;

namespace KiteCMS.Admin
{
    /// <summary>
    /// Summary description for user.
    /// </summary>
    public class User
	{

		private string username = "";
		private string fullname = "";
		private string encryptedPassword = "";
		private int userId = -1;
		private int active = 0;
		private int failedlogins = 0;
		private int changepassword = 0;
        private string[] shortcuts;
		private ValueCollection moduleAccess = new ValueCollection();
		private ValueCollection pageAccess = new ValueCollection();

		public User()
		{
		}

		public User(int userId)
		{
			UserData userData = new UserData();
			this.userId = userId;
			userData.Load(this);
		}

		public void Save()
		{
			UserData userData = new UserData();
			userData.Save(this);
		}

		public void Delete()
		{
			UserData userData = new UserData();
			userData.Delete(this);
		}

		public bool UsernameExists()
		{
			UserData userData = new UserData();
			return userData.UsernameExists(this);
		}

		public string Username
		{
			get
			{
				return username;
			}
			set
			{
				this.username = value;
			}
		}

		public string Fullname
		{
			get
			{
				return fullname;
			}
			set
			{
				this.fullname = value;
			}
		}

		public string EncryptedPassword
		{
			get
			{
				return encryptedPassword;
			}
			set
			{
					this.encryptedPassword = value;
			}
		}

		public string Password
		{
			// no get;
			set
			{
				if (this.username != null && this.username != "")
					this.encryptedPassword = Functions.Encrypt(value);
			}
		}

		public int UserId
		{
			get
			{
				return userId;
			}
			set
			{
				this.userId = value;
			}
		}

		public int Active
		{
			get
			{
				return active;
			}
			set
			{
				this.active = value;
			}
		}

		public int Failedlogins
		{
			get
			{
				return failedlogins;
			}
			set
			{
				this.failedlogins = value;
			}
		}

		public int Changepassword
		{
			get
			{
				return changepassword;
			}
			set
			{
				this.changepassword = value;
			}
		}

        public string[] Shortcuts
        {
            get { return shortcuts; }
            set { shortcuts = value; }
        }

		public ValueCollection ModuleAccess
		{
			get
			{
				return moduleAccess;
			}
			set
			{
				this.moduleAccess = value;
			}
		}

		public ValueCollection PageAccess
		{
			get
			{
				return pageAccess;
			}
			set
			{
				this.pageAccess = value;
			}
		}

	}
}
