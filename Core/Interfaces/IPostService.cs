﻿using CodeReview.Core.Models;

namespace CodeReview.Core.Interfaces;

public interface IPostService
{
    Post? Get(int id);

    List<Post>? GetAllFromUser(int ownerId);

    List<Post> Take(int amount, SortOrder sortOrder, params List<string>? filter);

    void Create(Post post);

    void Update(Post post);

    void Delete(int id);

    void Delete(Post post);

    void SaveChanges();
}