using CodeReview.Core.Models.EditorContent;
using System.Text.Json;

namespace CodeReview.Core.Handlers;

internal static class EditorContentHandler
{
	internal static string Process(EditorContent editorContent)
	{
		editorContent = RemoveEmptyBlocks(editorContent);

		return JsonSerializer.Serialize(editorContent);
	}

	private static EditorContent RemoveEmptyBlocks(EditorContent editorContent)
	{
		editorContent.Blocks.RemoveAll(block => string.IsNullOrWhiteSpace(block.Text));

		return editorContent;
	}
}