﻿using System.Threading.Tasks;

namespace AsyncAwait.Task2.CodeReviewChallenge.Services;

public interface IPrivacyDataService
{
    ValueTask<string> GetPrivacyDataAsync();
}
