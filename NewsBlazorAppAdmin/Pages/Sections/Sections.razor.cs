using Microsoft.AspNetCore.Components;
using NewsBlazorAppAdmin.Services;
using SharedLib.DTO;

namespace NewsBlazorAppAdmin.Pages.Sections
{
    public partial class Sections
    {
        public ResponseDTO<object> message { get; set; } = null;

        public IEnumerable<SectionDTO> sectionDTOs { get; set; }
        public SectionDTO ModelToEdit { get; set; }

        public SectionDTO ModelToAdd { get; set; }

        [Inject]
        public SectionsService SectionsService { get; set; }

        public async Task ShowMessage(ResponseDTO<object> message) {
            this.message = message;
            StateHasChanged();
            await Task.Delay(2000);
            this.message = null;
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            var response = await SectionsService.GetAllAsync();
            if (response.Success)
            {
                sectionDTOs = response.Data;
            }

        }
        public void OpenEdit(int id)
        {
            ModelToEdit = sectionDTOs.Where(x => x.Id == id).FirstOrDefault()?.Clone();
        }
        public void OpenAdd() {
            ModelToAdd = new SectionDTO { Id = 0 };
        }
        public async Task onSaveEdit(SectionDTO sectionDTO)
        {
            if (sectionDTO != null)
            {
                var response = await SectionsService.EditAsync(sectionDTO);
                if (response?.Success == true)
                {
                    sectionDTOs.Where(x => x.Id == sectionDTO.Id).FirstOrDefault().Name = sectionDTO.Name;
                }
                else
                {
                    _ = ShowMessage(new ResponseDTO<object>
                    {
                        Success = false,
                        Message = response.Message
                    });
                
                }
            }
            ModelToEdit = null;

        }
        public async Task onSaveAdd(SectionDTO sectionDTO)
        {
            if (sectionDTO != null)
            {
                var response = await SectionsService.AddAsync(sectionDTO);
                if (response?.Success == true)
                {
                    sectionDTO.Id = response.Data;
             
                    sectionDTOs = sectionDTOs.Prepend(sectionDTO);
                    _ = ShowMessage(new ResponseDTO<object>
                    {
                        Success = true,
                        Message = "تم اضافئه الفئه بنجاح"
                    });
                }
                else
                {
                    _ = ShowMessage(message = new ResponseDTO<object>
                    {
                        Success = false,
                        Message = response.Message
                    });
                }
            }
            ModelToAdd = null;
        }

        public async Task SetUp(int id) {
            var response = await SectionsService.SetUp(id);
            if (response?.Success == true)
            {
                var response2 = await SectionsService.GetAllAsync();
                if (response2.Success)
                {
                    sectionDTOs = response2.Data;
                    StateHasChanged();
                }
            }
        }

        public async Task SetDown(int id)
        {
            var response = await SectionsService.SetDown(id);
            if (response?.Success == true) {
                var response2 = await SectionsService.GetAllAsync();
                if (response2.Success)
                {
                    sectionDTOs = response2.Data;
                    StateHasChanged();
                }
            }

        }

    }
}
