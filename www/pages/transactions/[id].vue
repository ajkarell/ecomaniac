<template>
  <div
    v-if="transaction != null"
    class="w-7/12 px-4 py-2 text-center rounded-lg shadow-md bg-gray-50"
  >
    <h1 class="text-lg font-semibold">
      {{ transaction.description.length === 0 ? "-" : transaction.description }}
    </h1>
    <h1 class="text-md font-medium">
      {{ formatTransactionDate(transaction.date) }}
    </h1>
    <h1
      class="text-3xl font-bold"
      :class="{
        'text-green-600': transaction.amount > 0,
        'text-red-600': transaction.amount < 0,
      }"
    >
      {{ formatEuro(transaction.amount) }}
    </h1>
  </div>
</template>

<script setup lang="ts">
import { createApi } from "~~/api";
import { Transaction } from "~~/models/transaction";
import { formatEuro, formatTransactionDate } from "~~/formatting";

const route = useRoute();

const { getTransaction } = createApi();

const transaction = ref<Transaction | null>(null);

onMounted(async () => {
  const transactionId: number = Array.isArray(route.params.id)
    ? parseInt(route.params.id[0])
    : parseInt(route.params.id);
  const result = await getTransaction(transactionId);
  if (result.ok) {
    transaction.value = result.value;
  }
});
</script>
