import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

/**
 * Validator to ensure that at least one field from the given list is non-empty.
 * @param fields Array of field names to check.
 * @returns Validator function to be applied on the form group.
 */
export function atLeastOneFieldValidator(fields: string[]): ValidatorFn {
  return (formGroup: AbstractControl): ValidationErrors | null => {
    const hasAtLeastOneValue = fields.some((field) => {
      const control = formGroup.get(field);
      return control?.value && control.value.toString().trim() !== '';
    });

    return hasAtLeastOneValue ? null : { atLeastOneRequired: true };
  };
}
