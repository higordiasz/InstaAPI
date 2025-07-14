# Unofficial InstaAPI

It was created for educational purposes only.

A reverse-engineered implementation of the [Instagram](https://instagram.com/) web app's API.

**NOTE:** I can't guarantee you will not be blocked by using this method, although it has worked for me. Instagram does not allow bots or unofficial clients on their platform, so this shouldn't be considered totally safe.

## Quick Links

- [Discord](https://discord.gg/sYeya7g)
- [GitHub](https://github.com/higordiasz/InstaAPI)
- [Nuget](https://www.nuget.org/packages/InstaAPI/1.0.0)

## Installation

The module is now available on nuget!

`dotnet add package InstaAPI --version 1.0.0`

## Example usage

```c#
using InstaAPI;
using InstaAPI.Profile.Follow;
using InstaAPI.Profile.Login;
using InstaAPI.Model.Return;

namespace InstaApiUsage
{
  class Program
  {
    static async Task Main()
    {
      Instagram insta = new("username", "password");
      IReturn login = await insta.Login();
      if (login.Status == 1)
      {
        IReturn profileId = await insta.GetIdBySearchBar("cristiano");
        if (profileId.Status == 1)
        {
          IReturn follow = await insta.FollowUserById(profileId.Response);
          if (follow.Status == 1)
          {
            Console.WriteLine("Success to follow user");
          }
          else
          {
            Console.WriteLine(login.Response);
          }
        }
        else
        {
          Console.WriteLine(login.Response);
        }
      }
      else
      {
        Console.WriteLine("Failed to login");
        Console.WriteLine(login.Response);
      }
    }
  }
}
```

## Supported Features

### Authentication

| Function | Status | Description                 |
| -------- | ------ | --------------------------- |
| Login    | ❗✅   | Log in to Instagram account |

### Users & Relationships

| Function           | Status | Description                                             |
| ------------------ | ------ | ------------------------------------------------------- |
| FollowUserById     | ❗✅   | Follow a user by their userId                           |
| GetRelationById    | ❗✅   | Get relationship status between your account and a user |
| GetIdBySearchBar   | ❗✅   | Find a user via Instagram’s search bar                  |
| CheckUsername      | ❗❎   | Check if a username is available                        |
| CheckEmail         | ❗❎   | Check if an email is available                          |
| UsernameSuggestion | ❗❎   | Get username suggestions from Instagram                 |
| CreateAccount      | ❗❎   | Create a new Instagram account                          |

### Media & Content

| Function                   | Status | Description                            |
| -------------------------- | ------ | -------------------------------------- |
| GetUserProfileFromUsername | ❗✅   | Fetch profile info by username         |
| GetGender                  | ❗✅   | Retrieve account gender (if available) |
| LikeMediaByMediaId         | ❗✅   | Like a media item by its mediaId       |
| CommentMediaByMediaId      | ❗✅   | Post a comment on a media item         |
| SeeStorie                  | ❗✅   | View a story by its storyId            |
| SendStoriesLike            | ❗✅   | Like a story by its storyId            |
| StoriesFeedClass           | ❗✅   | Get a list of current stories          |

### Direct Messages (DM)

| Function   | Status | Description                      |
| ---------- | ------ | -------------------------------- |
| GetMyInbox | ❗✅   | Retrieve your inbox threads      |
| SeeMyInbox | ❗✅   | Mark your inbox messages as seen |

### Feed & Exploration

| Function   | Status | Description                          |
| ---------- | ------ | ------------------------------------ |
| –– None –– | ––     | No current features in this category |

---

## 🚧 Planned Features

We’ll implement these gradually:

### Authentication & Accounts

- `Logout()` – Log out of the current session
- `RefreshSession()` – Refresh the session token without logging back in

### Users & Relationships

- `GetUserFollowers(userId, pagination)` – List a user’s followers
- `GetUserFollowing(userId, pagination)` – List who a user is following
- `BlockUser(userId)` / `UnblockUser(userId)`
- `MuteUserStories(userId)` / `UnmuteUserStories(userId)`

### Media & Content

- `GetMediaByUser(userId, pagination)` – Retrieve a user’s media feed
- `GetMediaDetails(mediaId)` – Get post details (caption, location, etc.)
- `SaveMedia(mediaId)` / `UnsaveMedia(mediaId)`
- `UploadPhoto(filePath, caption)` – Upload a photo with a caption
- `UploadStory(filePath, caption)` – Upload a story with a caption

### Direct Messages (DM)

- `GetDirectThreads(pagination)` – List DM threads
- `SendDirectMessage(threadId, text)` – Send a message in a thread

### Feed & Exploration

- `GetTimelineFeed(pagination)` – Your main feed
- `GetExploreFeed(pagination)` – Explore feed
- `SearchHashtag(tag)` / `GetHashtagMedia(tag, pagination)`

### Stories & Highlights

- `GetStoryHighlights(userId)` – List a user’s story highlights
- `CreateHighlight(title, storyIds[])` – Create a new highlight
- `AddStoriesToHighlight(highlightId, storyIds[])` – Add stories to a highlight

## Contributing

Pull requests are welcome! If you see something you'd like to add, please do. For drastic changes, please open an issue first.

## Supporting the project

You can support the maintainer of this project through the links below

- [Support via GitHub Sponsors](https://github.com/sponsors/higordiasz)

## Disclaimer

This project is not affiliated, associated, authorized, endorsed by, or in any way officially connected with Instagram or any of its subsidiaries or its affiliates. The official Instagram website can be found at https://instagram.com. "Instagram" as well as related names, marks, emblems and images are registered trademarks of their respective owners.

## License

Copyright 2022 Higor D Zuqueto

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this project except in compliance with the License.
You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
