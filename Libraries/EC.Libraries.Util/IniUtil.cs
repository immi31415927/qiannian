namespace EC.Libraries.Util
{
    /// <summary>
    /// 读取INI配置文件的类
    /// </summary>
    public class IniUtil
    {

        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, System.Text.StringBuilder retVal, int size, string filePath);

        private string iniPath = "";


        /// <summary>
        /// Ini配置文件的存储路径
        /// </summary>
        public string IniPath
        {
            get { return iniPath; }
            set
            {
                if (System.IO.File.Exists(value))
                    iniPath = value;
            }
        }

        public IniUtil(string path)
        {
            this.IniPath = path;
        }

        /// <summary>
        /// 写入配置参数
        /// </summary>
        /// <param name="sectionName">配置类名</param>
        /// <param name="Key">键名</param>
        /// <param name="Value">键值</param>
        public void Write(string sectionName,string Key, string Value)
        {
            WritePrivateProfileString(sectionName, Key, Value, this.IniPath);
        }


        /// <summary>
        /// 读取配置参数
        /// </summary>
        /// <param name="sectionName">配置类名</param>
        /// <param name="Key">键名</param>
        /// <param name="defaultVal">缺省值</param>
        /// <returns>键值</returns>
        public string Read(string sectionName,string Key, string defaultVal)
        {
            System.Text.StringBuilder temp = new System.Text.StringBuilder();
            int i = GetPrivateProfileString(sectionName, Key, "", temp, 256, this.IniPath);
            if (i > 0)
                return temp.ToString();
            else
                return defaultVal;
        }

    }
}
