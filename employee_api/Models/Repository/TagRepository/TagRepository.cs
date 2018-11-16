using System.Data;
using Microsoft.Extensions.Configuration;

namespace DynamicReportAPI.Models.Repository.TagRepository {
    public class TagRepository {
        public IConfiguration _iconfiguration;
        TagModel _TagModel;
        public TagRepository (IConfiguration iconfiguration) {
            _iconfiguration = iconfiguration;
        }

        public DataSet insertUpdateTags (TagModel tagModel) {
            _TagModel = new TagModel (_iconfiguration);
            return _TagModel.insertUpdateTagData(tagModel).GetTableName();
        }

        
        public DataSet getTagList () {
            _TagModel = new TagModel (_iconfiguration);
            return _TagModel.getTagDataList().GetTableName();
        }
   
    }
}