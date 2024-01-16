using System.Globalization;
using Microsoft.JSInterop;
using System.Text.Json;
using System.Text.Encodings.Web;

namespace BitPlatformTemplate.Client.Core.Components.Pages;

public partial class CalendarPage
{
    public class RangeDate
    {
        public string Start { get; set; } = "";
        public string End { get; set; } = "";
    }

    [Inject]
    private IJSRuntime JS { get; set; }
    private RangeDate Range { get; set; }

    private bool IsOpen = false;
    private string EventTitle = "";
    private ElementReference CalendarRef;
    private DotNetObjectReference<CalendarPage>? objRef;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            objRef = DotNetObjectReference.Create(this);
            await JS.InvokeVoidAsync("createFullCalendar", CalendarRef);
            await JS.InvokeAsync<string>("setDotnetHelper", objRef);
        }
    }

    [JSInvokable]
    public async Task changeStatusModal(bool status, string range)
    {
        Range = JsonSerializer.Deserialize<RangeDate>(range);
        IsOpen = status;
        StateHasChanged();
    }

    public async Task setEvent()
    {
        var data = new { title = EventTitle, start = Range.Start, end = Range.End };
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        string strJson = JsonSerializer.Serialize(data, options);

        await JS.InvokeAsync<string>("addEvent", strJson);
        IsOpen = false;
    }
}
