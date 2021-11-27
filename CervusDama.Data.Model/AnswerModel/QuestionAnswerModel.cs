using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Model.AnswerModel
{
    public class QuestionAnswerModel
    {
        public int ID { get; set; }

        public string QuestionTitle { get; set; }

        public string Content { get; set; }

        public DateTime CreateAt { get; set; }

        public int LikeCount { get; set; }

        public int DislikeCount { get; set; }

        public byte Status { get; set; }

        public bool isSolved { get; set; }
        public UserModel.UserInfoModel UserInfo { get; set; }

    }
}
