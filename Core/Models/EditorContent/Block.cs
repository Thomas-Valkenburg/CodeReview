using Newtonsoft.Json;

namespace CodeReview.Core.Models.EditorContent;

public class Block
{
	[JsonProperty("key")]
	public string Key { get; set; }

	[JsonProperty("text")]
	public string Text { get; set; }

	[JsonProperty("type")]
	public string Type { get; set; }

	[JsonProperty("depth")]
	public int Depth { get; set; }

	[JsonProperty("inlineStyleRanges")]
	public List<object> InlineStyleRanges { get; set; }

	[JsonProperty("entityRanges")]
	public List<object> EntityRanges { get; set; }
}