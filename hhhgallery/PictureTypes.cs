using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hhhgallery
{
    // 定义图片的类别
    public enum PictureCategory
    {
        All, // 添加All选项
        Landscape,
        Portrait,
        Wildlife,
        Architectural,
        Abstract,
        Other
    }

    // 定义图片的作者类型
    public enum PictureAuthor
    {
        All, // 添加All选项
        Unknown,
        Photographer,
        Artist,
        Designer
    }
}
