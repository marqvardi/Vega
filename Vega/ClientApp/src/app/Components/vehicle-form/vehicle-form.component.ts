import { Component, OnInit } from '@angular/core';
import { VehicleService } from '../../Services/VehicleService.service';
import { ToastyService } from 'ng2-toasty';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/forkJoin';

@Component({
  selector: 'app-vehicle-form',
  templateUrl: './vehicle-form.component.html',
  styleUrls: ['./vehicle-form.component.css']
})
export class VehicleFormComponent implements OnInit {
  makes: any;
  models: any[];
  vehicle: any = {
    features: [],
    contact: {}

  };
  features: any;

  constructor(
    private vehicleService: VehicleService,
    private toastyService: ToastyService, 
    private route: ActivatedRoute,
    private router: Router) {
    route.params.subscribe(p => {
      this.vehicle.id = +p['id'];
    });
  }

  ngOnInit(): void {

    Observable.forkJoin([
      this.vehicleService.getMakes(),
      this.vehicleService.getVehicle(this.vehicle.id),
      this.vehicleService.getFeature()
    ]).subscribe(data => {
      this.makes = data[0];
      this.features = data[1];
      this.vehicle = data[2];      
    });

    //this.vehicleService.getVehicle(this.vehicle.id).subscribe(v => { this.vehicle = v; });
    //this.vehicleService.getMakes().subscribe(makes => this.makes = makes);
    //this.vehicleService.getFeature().subscribe(features => this.features = features);

  }

  onMakeChange() {
    const selectedMake = this.makes.find(m => m.id == this.vehicle.makeId);
    this.models = selectedMake ? selectedMake.models : [];
    delete this.vehicle.modelId;
  }

  onFeatureToggle(featureId, $event) {
    if ($event.target.checked)
      this.vehicle.features.push(featureId);
    else {
      var index = this.vehicle.features.indexOf(featureId);
      this.vehicle.features.splice(index, 1);
    }
  }

  submit() {
    this.vehicleService.create(this.vehicle)
      .subscribe(
        x => console.log(x));        
  }

}
