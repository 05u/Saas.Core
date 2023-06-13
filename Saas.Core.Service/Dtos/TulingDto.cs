namespace Saas.Core.Service.Dtos
{
    /// <summary>
    /// 图灵API发送模板
    /// </summary>
    public class TulingSendDto
    {
        public int reqType { get; set; } = 0;

        public Perception perception { get; set; }


        public UserInfo userInfo { get; set; }

    }

    /// <summary>
    /// 图灵API接收模板
    /// </summary>
    public class TulingReceiveDto
    {
        public List<Result> results { get; set; }
    }


    public class Result
    {
        public string groupType { get; set; }

        public string resultType { get; set; }

        public Values values { get; set; }
    }

    public class Values
    {
        public string text { get; set; }
    }






    public class Perception
    {
        public InputText inputText { get; set; }

        public SelfInfo selfInfo { get; set; }
    }

    public class InputText
    {
        public string text { get; set; }
    }

    public class SelfInfo
    {
        public Location location { get; set; }
    }

    public class Location
    {
        public string city { get; set; } = "南京";

        public string province { get; set; } = "江苏";
    }

    public class UserInfo
    {
        public string apiKey { get; set; } = "032b4700760cede07d872eed8ea562a5";

        public string userId { get; set; }
    }

    /// <summary>
    /// ChatGPT聊天接口入参
    /// </summary>
    public class ChatGPTInput
    {
        /// <summary>
        /// 模型
        /// </summary>
        public string model { get; set; } = "text-davinci-003";

        /// <summary>
        /// 消息
        /// </summary>
        public string prompt { get; set; }

        public int max_tokens { get; set; } = 2048;
        public int top_p { get; set; } = 1;

        public int frequency_penalty { get; set; } = 0;

        public double presence_penalty { get; set; } = 0.6;

        /// <summary>
        /// 人员标识 "stop": ["发消息的人", "AI"]
        /// </summary>
        public string stop { get; set; }

        public string user { get; set; }
    }

    /// <summary>
    /// ChatGPT聊天接口出参
    /// </summary>
    public class ChatGPTOutput
    {
        public string Id { get; set; }

        public string Object { get; set; }

        public int? Created { get; set; }

        public string Model { get; set; }

        public List<Choice> Choices { get; set; }

        public Usage Usage { get; set; }

        public Error Error { get; set; }
    }

    public class Choice
    {
        public string Text { get; set; }
        public int? Index { get; set; }

        public string Logprobs { get; set; }

        public string finish_reason { get; set; }

    }

    public class Usage
    {
        public int? Prompt_tokens { get; set; }

        public int? Completion_tokens { get; set; }

        public int? Total_tokens { get; set; }

    }

    public class Error
    {
        public string Message { get; set; }
        public string Type { get; set; }

        public string Param { get; set; }

        public string Code { get; set; }

    }
}
