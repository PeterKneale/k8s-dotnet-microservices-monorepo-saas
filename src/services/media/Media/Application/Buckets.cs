namespace Media.Application
{
    public static class Buckets
    {
        public const string Media = "media";

        /// <summary>
        /// IAM policy that allows only GetObject on the media bucket
        /// </summary>
        public const string MediaPolicy = "{ \"Version\": \"2012-10-17\", \"Statement\": [ { \"Effect\": \"Allow\", \"Principal\": { \"AWS\": [ \"*\" ] }, \"Action\": [ \"s3:GetObject\" ], \"Resource\": [ \"arn:aws:s3:::media/*\" ] } ]}";
    }
}