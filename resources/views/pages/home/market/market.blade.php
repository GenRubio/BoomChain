@extends('layouts.app')

@section('content')
   @foreach ($objetos as $objeto)
       <p>{{ $objeto->titulo }}</p>
       @if ($objeto->planta != null)
          <span> {{ $objeto->planta->creation_time }}</span>
       @endif
   @endforeach
@endsection