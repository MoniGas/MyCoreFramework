using System.Text;

namespace MCF.Common
{
    public class Utils
    {
        public static string GetPageList(int pageSize, int pageIndex, int totalCount, string linkUrl, int centSize)
        {
            //计算页数
            if (totalCount < 1 || pageSize < 1)
            {
                return "<span>共" + totalCount + "条记录</span>";
            }
            int pageCount = totalCount / pageSize;
            if (pageCount < 1)
            {
                return "<span>共" + totalCount + "条记录</span><a href=\"javascript:void()\" class=\"disabled\">首页</a><a href=\"javascript:void()\" class=\"disabled\">上一页</a><a href=\"javascript:void()\" class=\"d\">1</a><a href=\"javascript:void()\" class=\"disabled\">下一页</a><a href=\"javascript:void()\" class=\"disabled\">末页</a>";
            }
            if (totalCount % pageSize > 0)
            {
                pageCount += 1;
            }
            if (pageCount <= 1)
            {
                return "<span>共" + totalCount + "条记录</span><a href=\"javascript:void()\" class=\"disabled\">首页</a><a href=\"javascript:void()\" class=\"disabled\">上一页</a><a href=\"javascript:void()\" class=\"d\">1</a><a href=\"javascript:void()\" class=\"disabled\">下一页</a><a href=\"javascript:void()\" class=\"disabled\">末页</a>";
            }
            StringBuilder pageStr = new StringBuilder();
            string pageId = "__id__";
            string firstBtn = "<a href=\"javascript:void()\" onclick=\"" + linkUrl + "\" page=\"" + (pageIndex - 1) + "\">上一页</a>";
            string lastBtn = "<a href=\"javascript:void()\" onclick=\"" + linkUrl + "\" page=\"" + (pageIndex + 1) + "\">下一页</a>";
            string firstStr = "<a href=\"javascript:void()\" onclick=\"" + linkUrl + "\" page=\"1\">1</a>";
            string lastStr = "<a href=\"javascript:void()\" onclick=\"" + linkUrl + "\" page=\"" + pageCount.ToString() + "\">" + pageCount.ToString() + "</a>";
            string f_btn = "<a href=\"javascript:void()\" onclick=\"" + linkUrl + "\"  page=\"1\">首页</a>";
            string l_btn = "<a href=\"javascript:void()\" onclick=\"" + linkUrl + "\" page=\"" + pageCount.ToString() + "\">末页</a>";
            if (pageIndex <= 1)
            {
                f_btn = "<a href=\"javascript:void()\" class=\"disabled\">首页</a>";
                firstBtn = "<a href=\"javascript:void()\" class=\"disabled\">上一页</a>";
            }
            if (pageIndex >= pageCount)
            {
                lastBtn = "<a href=\"javascript:void()\" class=\"disabled\">下一页</a>";
                l_btn = "<a href=\"javascript:void()\" class=\"disabled\">末页</a>";
            }
            if (pageIndex == 1)
            {
                firstStr = "<a href=\"javascript:void()\" class=\"d\">1</a>";
            }
            if (pageIndex == pageCount)
            {
                lastStr = "<a href=\"javascript:void()\" class=\"d\">" + pageCount.ToString() + "</a>";
            }
            int firstNum = pageIndex - centSize / 2; //中间开始的页码
            if (pageIndex < centSize)
                firstNum = 2;
            int lastNum = pageIndex + centSize - (centSize / 2 + 1); //中间结束的页码
            if (lastNum >= pageCount)
                lastNum = pageCount - 1;
            pageStr.Append("<span>共" + totalCount + "条记录</span>");
            pageStr.Append(f_btn + firstBtn + firstStr);
            if (pageIndex >= centSize)
            {
                pageStr.Append("<a href=\"javascript:void()\">...</a>\n");
            }
            for (int i = firstNum; i <= lastNum; i++)
            {
                if (i == pageIndex)
                {
                    pageStr.Append("<a href=\"javascript:void()\" class=\"d\">" + i + "</a>");
                }
                else
                {
                    pageStr.Append("<a href=\"javascript:void()\" onclick=\"" + linkUrl + "\"  page=\"" + i + "\">" + i + "</a>");
                }
            }
            if (pageCount - pageIndex > centSize - centSize / 2)
            {
                pageStr.Append("<a href=\"javascript:void()\">...</a>");
            }
            pageStr.Append(lastStr + lastBtn + l_btn);
            return pageStr.ToString();
        }
    }
}
