// Fill out your copyright notice in the Description page of Project Settings.

//#include "PaperTileMapComponent.h"
#include "CardToTapManager.h"

// Sets default values
ACardToTapManager::ACardToTapManager()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;

}

// Called when the game starts or when spawned
void ACardToTapManager::BeginPlay()
{
	Super::BeginPlay();
	
}

// Called every frame
void ACardToTapManager::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}

void ACardToTapManager::GetTile()
{
	FVector WorldLocation;
	FVector WorldDirection;
	APlayerController* PlayerController = GetWorld()->GetFirstPlayerController();
	if (PlayerController != nullptr)
	{
		PlayerController->DeprojectMousePositionToWorld(WorldLocation, WorldDirection);
	}
	FHitResult HitResult;
	if (GetWorld()->LineTraceSingleByChannel(HitResult, WorldLocation, WorldLocation - WorldDirection * 100, ECC_Visibility))
	{
		//UTileMap* Tilemap = YourTilemapComponent->GetTileMap();
		//FIntPoint TileLocation = Tilemap->WorldToTile(HitResult.ImpactPoint);
	}
}

