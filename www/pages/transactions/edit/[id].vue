<template>
  <div>
    <form
      v-show="isOk"
      @submit.prevent="saveAndExit"
      class="px-8 py-4 rounded-lg shadow border border-gray-300"
    >
      <div class="mb-4 flex flex-col">
        <div class="pb-2">
          <label class="block text-gray-700 font-bold mb-2" for="date">
            Date
          </label>
          <input
            v-model="transactionDateString"
            class="shadow appearance-none border rounded py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="date"
            type="date"
          />
        </div>
        <div class="pb-2">
          <label class="block text-gray-700 font-bold mb-2" for="amount">
            Amount
          </label>
          <input-currency
            v-model="transactionAmount"
            ref="amountInputRef"
            class="shadow appearance-none border rounded py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="amount"
          />
        </div>
        <div class="">
          <label class="block text-gray-700 font-bold mb-2" for="description">
            Description
          </label>
          <input
            v-model="transactionDescription"
            class="shadow appearance-none border rounded py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="description"
            type="text"
            placeholder="Description"
          />
        </div>
      </div>
      <button
        type="submit"
        class="mr-2 px-4 py-2 bg-blue-500 hover:bg-blue-700 text-white rounded-lg border border-blue-700"
      >
        Save
      </button>
      <button
        @click.prevent="deleteAndExit()"
        class="mr-2 px-4 py-2 bg-red-500 hover:bg-red-700 text-white rounded-lg border border-red-700"
      >
        Delete
      </button>
      <button
        @click.prevent="navigateTo('/')"
        class="px-4 py-2 text-gray-700 rounded-lg border border-gray-700"
      >
        Cancel
      </button>
    </form>
    <h1 v-show="isOk === false" class="text-xl text-red-700">
      Transaction with ID {{ transactionId }} not found.
    </h1>
  </div>
</template>

<script setup lang="ts">
import { format, parseISO } from "date-fns";
import { createApi } from "~~/api";
import { Transaction } from "~~/models/transaction";

const { getTransaction, putTransaction, deleteTransaction } = createApi();

const today = new Date();
today.setUTCHours(0, 0, 0, 0);

const transactionDateString = ref<string>(format(today, "yyyy-MM-dd"));
const transactionDate = computed(() => {
  return parseISO(transactionDateString.value);
});

const transactionAmount = ref(0.0);
const transactionDescription = ref("");

const route = useRoute();
const transactionId: number = Array.isArray(route.params.id)
  ? parseInt(route.params.id[0])
  : parseInt(route.params.id);

const isOk = ref<boolean | null>(null);

const amountInputRef = ref(null);

async function saveAndExit() {
  const response = await putTransaction({
    id: transactionId,
    date: transactionDate.value,
    amount: transactionAmount.value,
    description: transactionDescription.value,
  } as Transaction);

  if (response.ok) {
    navigateTo("/");
  } else {
    alert(await response.json());
  }
}

async function deleteAndExit() {
  const response = await deleteTransaction(transactionId);

  if (response.ok) {
    navigateTo("/");
  } else {
    alert(await response.json());
  }
}

onMounted(async () => {
  const result = await getTransaction(transactionId);
  if (result.ok) {
    const transaction = result.value;
    transactionDateString.value = format(transaction.date, "yyyy-MM-dd");
    transactionAmount.value = transaction.amount;
    transactionDescription.value = transaction.description;
  }
  isOk.value = result.ok;

  await nextTick();

  amountInputRef.value.select();
});
</script>
