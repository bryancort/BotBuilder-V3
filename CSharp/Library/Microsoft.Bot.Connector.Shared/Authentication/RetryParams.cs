﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Bot.Connector.Shared.Authentication
{
    public class RetryParams
    {
        private const int maxRetries = 8;
        private static readonly TimeSpan MaxDelay = TimeSpan.FromSeconds(10);
        private static readonly TimeSpan DefaultBackOffTime = TimeSpan.FromMilliseconds(50);

        public static RetryParams StopRetrying { get; } = new RetryParams() { ShouldRetry = false };

        public bool ShouldRetry { get; set; }
        public TimeSpan RetryAfter { get; set; }

        public RetryParams() { }

        public RetryParams(TimeSpan retryAfter, bool shouldRetry = true)
        {
            ShouldRetry = shouldRetry;
            RetryAfter = retryAfter;

            // We don't allow more than maxDelaySeconds seconds delay.
            if (RetryAfter > MaxDelay)
            {
                // We don't want to throw here though - if the server asks for more delay
                // than we are willing to, just enforce the upper bound for the delay 
                RetryAfter = MaxDelay;
            }
        }

        public static RetryParams DefaultBackOff(int retryCount)
        {
            if (retryCount < maxRetries)
            {
                return new RetryParams(DefaultBackOffTime);
            }
            else
            {
                return RetryParams.StopRetrying;
            }
        }
    }
}
