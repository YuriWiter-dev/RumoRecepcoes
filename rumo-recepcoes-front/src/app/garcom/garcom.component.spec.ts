import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GarcomComponent } from './garcom.component';

describe('GarcomComponent', () => {
  let component: GarcomComponent;
  let fixture: ComponentFixture<GarcomComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GarcomComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(GarcomComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
