namespace MCF.Common
{
    /// <summary>
    /// 公共枚举类
    /// </summary>
    public class CommonEnum
    {
        /// <summary>
        /// 删除状态
        /// </summary>
        public enum DeleteState
        {
            未删除 = 0, 已删除
        }

        /// <summary>
        /// 性别
        /// </summary>
        public enum Sex
        {
            男 = 0, 女, 其他
        }

        /// <summary>
        /// 操作类型
        /// </summary>
        public enum OpertionType
        {
            添加 = 1, 编辑, 删除
        }

    }
}
