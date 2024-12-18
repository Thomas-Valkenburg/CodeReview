using Newtonsoft.Json;

namespace CodeReview.Core.Models.EditorContent;

public class EditorContent
{
	[JsonProperty("blocks")]
	public List<Block> Blocks { get; set; }

	[JsonProperty("entityMap")]
	public EntityMap EntityMap { get; set; }
}