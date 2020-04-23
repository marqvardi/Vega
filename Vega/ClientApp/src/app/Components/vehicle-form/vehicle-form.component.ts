import { Component, OnInit } from '@angular/core';
import { VehicleService } from '../../Services/VehicleService.service';

@Component({
  selector: 'app-vehicle-form',
  templateUrl: './vehicle-form.component.html',
  styleUrls: ['./vehicle-form.component.css']
})
export class VehicleFormComponent implements OnInit {
  makes: any;
  models: any[];
  vehicle: any = {};
  features: any;

  constructor(private vehicleService: VehicleService ) { }

  ngOnInit(): void {
    this.vehicleService.getMakes().subscribe(makes => this.makes = makes);
    this.vehicleService.getFeature().subscribe(features => this.features = features);

  }

  onMakeChange() {
    var selectedMake = this.makes.find(m => m.id == this.vehicle.make);
    this.models = selectedMake ? selectedMake.models : [];
  }

}
