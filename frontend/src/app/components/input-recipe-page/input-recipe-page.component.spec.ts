import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InputRecipePageComponent } from './input-recipe-page.component';

describe('InputRecipePageComponent', () => {
  let component: InputRecipePageComponent;
  let fixture: ComponentFixture<InputRecipePageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InputRecipePageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InputRecipePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
