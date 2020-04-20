using TMPro;
using UnityEngine;

public class MissionOverCanvasController : MonoBehaviour
{
	public Canvas Canvas;
	public TextMeshProUGUI Text;

	private const string CowardText = "Well what can you say about this attempt? You tried, I guess. And by tried I mean chickened out at the first opportunity. You do realize people stayed awake until 3am to make this game? And yet you play it like this...";
	private const string SuccessText = "It's an honor watching you work, like a modern day Da Vinci or Shakespear. Take a bow, you magnificent human being!";
	private const string DefeatText = "A noble attempt, but a sub-par execution. We'll try again once your prison sentance is over, shouldn't be more than a few years with good behaviour.";
	private const string CreatureCapturedText = "The guards captured the poor thing and brought him to a maximum security cell. He'll spend the rest of his life on a giant hamster wheel. Fortunately, I don't think he'll live all that long...";

	private void Awake()
	{
		Canvas.enabled = false;
	}

	public void Display(Outcome outcome)
	{
		Time.timeScale = 0.0f;
		Text.text = OutcomeText(outcome);
		Canvas.enabled = true;
	}

	private string OutcomeText(Outcome outcome)
	{
		switch(outcome)
		{
			case Outcome.Coward:
				return CowardText;
			case Outcome.CreatureCaptured:
				return CreatureCapturedText;
			case Outcome.Defeat:
				return DefeatText;
			case Outcome.Success:
				return SuccessText;
		}
		throw new UnityException($"Uknown outcome {outcome.ToString()}");
	}
}

public enum Outcome
{
	Coward = 1,
	Success = 2,
	Defeat = 3,
	CreatureCaptured = 4
}