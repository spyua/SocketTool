using System;
using System.Threading;
using System.Threading.Tasks;

namespace SocketAsyncUtility.Core
{
    /**
     * Author       : SPYUA.YU
     * Date         : 2020/12/12
     * Description  : Socket 的擴充方法。
     * Reference    : 
     * Modified     : 
     */
    public static class CompareHelper
    {
        /// <summary>
        ///     比較來源值是否等於比較值。
        /// </summary>
        /// <param name="sourceVal"> 來源值。 </param>
        /// <param name="result"> 輸出結果。(即來源值) </param>
        /// <param name="comparisonVal"> 比較值。 </param>
        /// <returns> 比較結果。 </returns>
        public static bool EqualTo(this int sourceVal, out int result, int comparisonVal)
            => (result = sourceVal) == comparisonVal;

        /// <summary>
        ///     比較來源值是否大於(等於)比較值。
        /// </summary>
        /// <param name="sourceVal"> 來源值。 </param>
        /// <param name="result"> 輸出結果。(即來源值) </param>
        /// <param name="comparisonVal"> 比較值。 </param>
        /// <param name="isEqual"> 是否要作等於。 </param>
        /// <returns> 比較結果。 </returns>
        public static bool BiggerThan(this int sourceVal, out int result, int comparisonVal, bool isEqual = false)
            => isEqual ? (result = sourceVal) >= comparisonVal : (result = sourceVal) > comparisonVal;

        /// <summary>
        ///     比較來源值是否小於(等於)比較值。
        /// </summary>
        /// <param name="sourceVal"> 來源值。 </param>
        /// <param name="result"> 輸出結果。(即來源值) </param>
        /// <param name="comparisonVal"> 比較值。 </param>
        /// <param name="isEqual"> 是否要作等於。 </param>
        /// <returns> 比較結果。 </returns>
        public static bool SmallerThan(this int sourceVal, out int result, int comparisonVal, bool isEqual = false)
            => isEqual ? (result = sourceVal) <= comparisonVal : (result = sourceVal) < comparisonVal;

        /// <summary>
        ///     註冊等待取消工作。
        /// </summary>
        /// <param name="task"> Task。 </param>
        /// <param name="cancellationToken"> 工作取消令牌。 </param>
        /// <returns> Task。 </returns>
        public static async Task WithCancellation(this Task task, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();

            using (cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).TrySetResult(true), tcs))
                if (task != await Task.WhenAny(task, tcs.Task))
                    throw new OperationCanceledException(cancellationToken);

            await task;
        }

        /// <summary>
        ///     註冊等待取消工作。
        /// </summary>
        /// <typeparam name="T"> 要回傳的型別。 </typeparam>
        /// <param name="task"> Task。 </param>
        /// <param name="cancellationToken"> 工作取消令牌。 </param>
        /// <returns> 執行的結果。 </returns>
        public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();

            using (cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).TrySetResult(true), tcs))
                if (task != await Task.WhenAny(task, tcs.Task))
                    throw new OperationCanceledException(cancellationToken);

            return await task;
        }
    }
}
