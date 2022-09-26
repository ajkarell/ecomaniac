<template>
  <input type="text" ref="inputRef" />
</template>

<script setup lang="ts">
import { CurrencyInputOptions, useCurrencyInput } from "vue-currency-input";

const select = () => {
  inputRef.value.select();
};

defineExpose({ select });

const props = defineProps<{
  modelValue: number;
}>();

const options = { currency: "EUR", locale: "fi-FI" } as CurrencyInputOptions;
const { inputRef, setValue } = useCurrencyInput(options);

watch(
  () => props.modelValue,
  (newValue) => {
    setValue(newValue);
  }
);
</script>
