using Microsoft.AspNetCore.Components;

namespace FunderMaps.Incident.Components;

public class State
{
    public int StepId { get; set; } = 0;
    public bool ShowProgressSteps => StepId > 0 && StepId < 10;
    public bool ShowNavPrev => StepId > 0 && StepId < 10;
    public bool ShowNavNext => StepId < 10;
    public bool DisableNavNext { get; set; } = true;
    public string? Vendor { get; set; }
    public bool Feedback { get; set; } = false;

    public Core.Entities.Incident Model { get; set; } = new();

    public EventCallback OnNextClick;

    public State()
    {
        Model.FoundationType = Core.Types.FoundationType.Other;
        Model.ChainedBuilding = true;
        Model.Owner = true;
        Model.NeighborRecovery = false;
        Model.ClientId = 10;
    }

    public string getProgressStepClass(int step)
    {
        if (StepId == step)
        {
            return "ProgressSteps__Current";
        }
        else if (StepId > step)
        {
            return "ProgressSteps__Finished";
        }
        return "ProgressSteps__Future";
    }

    public string NavLinkPrevPage()
    {
        string uri = "/";

        if (Feedback)
        {
            switch (StepId)
            {
                case 0:
                    uri = "/";
                    break;
                case 1:
                    uri = "/";
                    break;
                case 4:
                    uri = "/survey/address";
                    break;
                case 5:
                    uri = "/survey/feedback-characteristics";
                    break;
                case 7:
                    uri = "/survey/foundation-type";
                    break;
                case 8:
                    uri = "/survey/upload";
                    break;
                case 9:
                    uri = "/survey/note";
                    break;
                case 10:
                    uri = "/survey/contact";
                    break;
            }
        }
        else
        {
            switch (StepId)
            {
                case 0:
                    uri = "/";
                    break;
                case 1:
                    uri = "/";
                    break;
                case 2:
                    uri = "/survey/address";
                    break;
                case 3:
                    uri = "/survey/foundation-damage-cause";
                    break;
                case 4:
                    uri = "/survey/foundation-damage-characteristics";
                    break;
                case 5:
                    uri = "/survey/address-characteristics";
                    break;
                case 6:
                    uri = "/survey/foundation-type";
                    break;
                case 7:
                    uri = "/survey/environment-damage-characteristics";
                    break;
                case 8:
                    uri = "/survey/upload";
                    break;
                case 9:
                    uri = "/survey/note";
                    break;
                case 10:
                    uri = "/survey/contact";
                    break;
            }
        }

        return uri;
    }

    public string NavLinkNextPage()
    {
        string uri = "/";

        if (Feedback)
        {
            switch (StepId)
            {
                case 0:
                    uri = "/survey/address";
                    break;
                case 1:
                    uri = "/survey/feedback-characteristics";
                    break;
                case 4:
                    uri = "/survey/foundation-type";
                    break;
                case 5:
                    uri = "/survey/upload";
                    break;
                case 7:
                    uri = "/survey/note";
                    break;
                case 8:
                    uri = "/survey/contact";
                    break;
                case 9:
                    uri = "/survey/finish";
                    break;
            }
        }
        else
        {
            switch (StepId)
            {
                case 0:
                    uri = "/survey/address";
                    break;
                case 1:
                    uri = "/survey/foundation-damage-cause";
                    break;
                case 2:
                    uri = "/survey/foundation-damage-characteristics";
                    break;
                case 3:
                    uri = "/survey/address-characteristics";
                    break;
                case 4:
                    uri = "/survey/foundation-type";
                    break;
                case 5:
                    uri = "/survey/environment-damage-characteristics";
                    break;
                case 6:
                    uri = "/survey/upload";
                    break;
                case 7:
                    uri = "/survey/note";
                    break;
                case 8:
                    uri = "/survey/contact";
                    break;
                case 9:
                    uri = "/survey/finish";
                    break;
            }
        }

        return uri;
    }

    public string NavTextNextPage()
    {
        if (StepId == 0)
        {
            return "Melding maken of vraag stellen";
        }
        if (StepId == 9)
        {
            return "Versturen";
        }

        return "Volgende";
    }

    public bool HoldNavNext()
    {
        return StepId switch
        {
            1 => true,
            9 => true,
            _ => false,
        };
    }
}
