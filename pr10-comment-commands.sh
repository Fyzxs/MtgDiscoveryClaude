# Azure DevOps PR Comment Commands for PR #10

# 1. Post Summary Comment
az repos pr thread create --pull-request-id 10 \
  --content "" \
  --status Active

# 2. Post Inline Comment for UserCardItem Interface Suggestion
az devops invoke --area git --resource pullRequestThreads \
  --org https://dev.azure.com/fyzxs \
  --route-parameters project=MtgDiscovery repositoryId=MtgDiscoveryVibe pullRequestId=10 \
  --http-method POST --api-version 6.0 \
  --in-file pr10-inline-comment-1.json

# 3. Post Inline Comment for CollectedItem Validation
az devops invoke --area git --resource pullRequestThreads \
  --org https://dev.azure.com/fyzxs \
  --route-parameters project=MtgDiscovery repositoryId=MtgDiscoveryVibe pullRequestId=10 \
  --http-method POST --api-version 6.0 \
  --in-file pr10-inline-comment-2.json

# 4. Post Positive Feedback Comment
az devops invoke --area git --resource pullRequestThreads \
  --org https://dev.azure.com/fyzxs \
  --route-parameters project=MtgDiscovery repositoryId=MtgDiscoveryVibe pullRequestId=10 \
  --http-method POST --api-version 6.0 \
  --in-file pr10-positive-feedback.json

