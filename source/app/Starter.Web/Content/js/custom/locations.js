function MoveLocationModel(jsonModel) {
    var self = this;
    self.model = ko.mapping.fromJS(jsonModel);

    self.selectRegion = function(item) {
        self.model.RegionId(item.Value());
        self.model.SubRegionId(null);
        self.model.CompanyId(null);
    };
    
    self.selectSubRegion = function(item) {
        self.model.SubRegionId(item.Value());
        self.model.CompanyId(null);
        self.model.RegionId(null);
    }; 
    
    self.selectCompany = function(item) {
        self.model.CompanyId(item.Value());
        self.model.RegionId(null);
        self.model.SubRegionId(null);
    };
    
    self.save = function (formElement) {
        ko.utils.postModel(formElement, self.model);
    };
}